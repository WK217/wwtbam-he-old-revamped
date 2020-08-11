using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.ViewModel
{
    public sealed class SoundOutDevicesViewModel : ReadOnlyReactiveCollection<SoundOutDeviceViewModel>
    {
        public SoundOutDevicesViewModel()
        {
            AudioManager.Instance.Collection.ToObservableChangeSet()
                                            .Transform(device => new SoundOutDeviceViewModel(device))
                                            .Bind(_collection)
                                            .Subscribe();
        }
    }
}