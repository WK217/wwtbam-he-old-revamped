using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.ViewModel;

public sealed class AudioViewModel : ReactiveObject
{
    public AudioViewModel()
    {
        this.WhenAnyValue(vm => vm.Audio)
            .Where(audio => audio is not null)
            .Select(audio => GetDescription(audio))
            .BindTo(this, x => x.Description);

        PlayAudioCommand = ReactiveCommand.Create(() =>
        {
            if (Audio is not null)
                AudioManager.Play((Audio)Audio);
        });
    }

    public AudioViewModel(Audio audio)
        : this()
    {
        Audio = audio;
    }

    private string GetDescription(object obj)
    {
        MemberInfo[] member = obj.GetType().GetMember(obj.ToString());

        if (member is not null && member.Length != 0)
        {
            object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes is not null && customAttributes.Length != 0)
                return ((DescriptionAttribute)customAttributes[0]).Description;
        }

        return obj.ToString();
    }

    [Reactive] public Audio? Audio { get; set; }
    [Reactive] public string Description { get; set; }

    public ReactiveCommand<Unit, Unit> PlayAudioCommand { get; }
}
