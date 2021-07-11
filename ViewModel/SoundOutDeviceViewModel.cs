using NAudio.CoreAudioApi;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.ViewModel
{
    public sealed class SoundOutDeviceViewModel : ReactiveObject
    {
        #region Fields

        private readonly AudioManager _audioManager;
        private readonly MMDevice _device;
        private readonly ObservableAsPropertyHelper<bool> _isSelected;

        #endregion Fields

        public SoundOutDeviceViewModel(MMDevice device)
        {
            _audioManager = AudioManager.Instance;
            _device = device;

            _isSelected = this.WhenAnyValue(x => x._audioManager.SelectedSoundOutDevice)
                              .Select(device => device == _device)
                              .ToProperty(this, nameof(IsSelected));

            SelectSoundOutCommand = ReactiveCommand.Create(() => { _audioManager.SelectedSoundOutDevice = _device; });
        }

        #region Properties

        public string Description => _device.FriendlyName;
        public bool IsSelected => _isSelected.Value;

        #endregion Properties

        #region Commands

        public ReactiveCommand<Unit, Unit> SelectSoundOutCommand { get; }

        #endregion Commands
    }
}