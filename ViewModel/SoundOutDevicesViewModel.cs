using NAudio.CoreAudioApi;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.ViewModel;

public sealed class SoundOutDevicesViewModel : ReadOnlyReactiveCollection<SoundOutDeviceViewModel>
{
    public SoundOutDevicesViewModel()
        : base()
    {
        foreach (MMDevice device in AudioManager.Instance.SoundOutDevices)
            _collection.Add(new SoundOutDeviceViewModel(device));
    }
}
