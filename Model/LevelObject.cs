using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media;

namespace WwtbamOld.Model
{
    public class LevelObject : ReactiveObject
    {
        #region Fields

        private readonly Func<byte, bool> _levelsRange;
        private readonly string _imageNameFormat;
        private ObservableAsPropertyHelper<ImageSource> _image;

        private List<byte> _demoValues;
        private byte _demoCounter;
        private byte _demoOriginalLevel;
        private IDisposable _demoSubscription;

        #endregion Fields

        public LevelObject(Game game, string imageNameFormat, Func<byte, bool> levelsRange)
        {
            _imageNameFormat = imageNameFormat;
            _levelsRange = levelsRange;

            _image = this.WhenAnyValue(x => x.Level)
                         .Where(_levelsRange)
                         .Select(lvl => ResourceManager.GetImageSource(string.Format(_imageNameFormat, lvl)))
                         .ToProperty(this, nameof(Image));
        }

        #region Properties

        [Reactive] public bool IsShown { get; set; }
        [Reactive] public byte Level { get; set; }
        public ImageSource Image => _image.Value;

        public void StartDemo()
        {
            _demoCounter = 0;
            _demoOriginalLevel = Level;

            _demoValues = new List<byte>();

            for (byte i = 0; i <= byte.MaxValue; i++)
            {
                if (_levelsRange(i))
                {
                    if (_demoOriginalLevel == 0 && i == 0)
                        continue;
                    _demoValues.Add(i);
                }
                else
                    break;
            }

            _demoValues.Add(_demoOriginalLevel);

            _demoSubscription = Observable.Interval(TimeSpan.FromMilliseconds(750), RxApp.MainThreadScheduler)
                                          .TakeWhile(v => _demoCounter < _demoValues.Count)
                                          .Subscribe(v => Level = _demoValues[_demoCounter++]);
        }

        public void StopDemo()
        {
            _demoSubscription.Dispose();
            Level = _demoOriginalLevel;
        }

        #endregion Properties
    }
}