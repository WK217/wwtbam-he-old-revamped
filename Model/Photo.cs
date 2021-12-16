using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WwtbamOld.Model
{
    public sealed class Photo : ReactiveObject
    {
        #region Fields

        private readonly Game _game;
        private readonly ObservableAsPropertyHelper<ImageSource> _photoImage;
        private readonly ObservableAsPropertyHelper<bool> _canShowImage;

        #endregion Fields

        public Photo(Game game)
        {
            _game = game;

            _photoImage = this.WhenAnyValue(x => x.PhotoUrl)
                              .Throttle(TimeSpan.FromMilliseconds(300))
                              .Where(url => !string.IsNullOrWhiteSpace(url))
                              .Select(url => (ImageSource)new BitmapImage(new Uri(url)))
                              .ObserveOn(RxApp.MainThreadScheduler)
                              .ToProperty(this, nameof(PhotoImage));

            _canShowImage = this.WhenAnyValue(x => x.PhotoImage)
                                .Select(image => image != null)
                                .ObserveOn(RxApp.MainThreadScheduler)
                                .ToProperty(this, nameof(CanShowImage));

            this.WhenAnyValue(x => x._game.CurrentQuiz.Question.Photo)
                .BindTo(this, x => x.PhotoUrl);
        }

        #region Properties

        [Reactive] public string PhotoUrl { get; set; }
        public ImageSource PhotoImage => _photoImage.Value;

        public bool CanShowImage => _canShowImage.Value;

        [Reactive] public bool IsBigShown { get; set; }
        [Reactive] public bool IsSmallShown { get; set; }

        #endregion Properties
    }
}