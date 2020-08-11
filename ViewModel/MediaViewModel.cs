using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using WwtbamOld.Media.Audio;
using WwtbamOld.View;

namespace WwtbamOld.ViewModel
{
    public sealed class MediaViewModel : ViewModelBase
    {
        #region Fields

        private IDisposable _commOutSubscription, _closingTitlesSubscription;

        #endregion Fields

        public MediaViewModel(HostViewModel hostViewModel)
        {
            PreLoop = new AudioViewModel(Audio.PreLoop);

            Opening = new AudioViewModel(Audio.Opening);
            Opening.PlayAudioCommand.Subscribe(_ =>
            {
                hostViewModel.IsLogoShown = true;
            });

            Meeting = new AudioViewModel(Audio.Meeting);
            BackgroundGeneral = new AudioViewModel(Audio.BackgroundGeneral);

            RulesExplanation = new AudioViewModel();
            RulesExplanation.PlayAudioCommand.Subscribe(_ =>
            {
                RulesExplanationState += 1;
            });

            this.WhenAnyValue(media => media.RulesExplanationState).Subscribe(state =>
            {
                switch (RulesExplanationState)
                {
                    case RulesExplanationState.Begin:
                        RulesExplanation.Description = "Демонстрация дерева";
                        AudioManager.Instance.Play(Audio.RulesExplanation);
                        hostViewModel.IsLogoShown = false;
                        hostViewModel.BigMoneyTree.IsShown = true;
                        break;

                    case RulesExplanationState.DemoTree:
                        RulesExplanation.Description = "Окончание объяснения правил игры";
                        hostViewModel.IsLogoShown = false;
                        hostViewModel.BigMoneyTree.IsShown = true;
                        hostViewModel.BigMoneyTree.StartDemo();
                        break;

                    case RulesExplanationState.End:
                        AudioManager.Instance.Play(Audio.RulesEnd);
                        hostViewModel.IsLogoShown = false;
                        hostViewModel.BigMoneyTree.IsShown = false;
                        hostViewModel.BigMoneyTree.StopDemo();
                        RulesExplanationState = RulesExplanationState.None;
                        break;

                    default:
                        RulesExplanation.Description = "Правила игры";
                        break;
                }
            });

            CommercialIn = new AudioViewModel(Audio.CommercialIn);
            CommercialIn.PlayAudioCommand.Subscribe(_ =>
            {
                hostViewModel.IsLogoShown = true;
            });

            CommercialOut = new AudioViewModel(Audio.CommercialOut);
            CommercialOut.PlayAudioCommand.Subscribe(_ =>
            {
                _commOutSubscription?.Dispose();
                _commOutSubscription = Observable.Timer(TimeSpan.FromMilliseconds(5000), RxApp.MainThreadScheduler)
                                                 .Subscribe(_ => { hostViewModel.IsLogoShown = false; });
            });

            Goodbye = new AudioViewModel(Audio.Goodbye);
            Goodbye.PlayAudioCommand.Subscribe(_ =>
            {
                hostViewModel.ClearScreen();
                hostViewModel.Winnings.IsShown = true;
            });

            FinalSiren = new AudioViewModel(Audio.FinalSiren);

            ClosingTitles = new AudioViewModel(Audio.Closing);
            ClosingTitles.PlayAudioCommand.Subscribe(_ =>
            {
                _closingTitlesSubscription?.Dispose();
                _closingTitlesSubscription = Observable.Timer(TimeSpan.FromMilliseconds(80000), RxApp.MainThreadScheduler)
                                                       .Subscribe(_ => { hostViewModel.IsLogoShown = true; });
            });
        }

        [Reactive] public RulesExplanationState RulesExplanationState { get; set; }

        public AudioViewModel PreLoop { get; }
        public AudioViewModel Opening { get; }
        public AudioViewModel Meeting { get; }
        public AudioViewModel BackgroundGeneral { get; }

        public AudioViewModel RulesExplanation { get; }

        public AudioViewModel CommercialIn { get; }
        public AudioViewModel CommercialOut { get; }
        public AudioViewModel Goodbye { get; }

        public AudioViewModel FinalSiren { get; }
        public AudioViewModel ClosingTitles { get; }
    }

    public sealed class AudioViewModel : ViewModelBase
    {
        public AudioViewModel()
        {
            this.WhenAnyValue(vm => vm.Audio)
                .Where(audio => audio != null)
                .Select(audio => GetDescription(audio))
                .BindTo(this, x => x.Description);

            PlayAudioCommand = ReactiveCommand.Create(() =>
            {
                if (Audio != null)
                    AudioManager.Instance.Play((Audio)Audio);
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

            if (member != null && member.Length != 0)
            {
                object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (customAttributes != null && customAttributes.Length != 0)
                    return ((DescriptionAttribute)customAttributes[0]).Description;
            }

            return obj.ToString();
        }

        [Reactive] public Audio? Audio { get; set; }
        [Reactive] public string Description { get; set; }

        public ReactiveCommand<Unit, Unit> PlayAudioCommand { get; }
    }

    public enum RulesExplanationState : byte
    {
        None,
        Begin,
        DemoTree,
        End
    }
}