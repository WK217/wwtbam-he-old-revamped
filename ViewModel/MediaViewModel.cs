using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.ViewModel;

public sealed class MediaViewModel : ViewModelBase
{
    #region Fields

    private IDisposable _commOutSubscription, _closingTitlesSubscription;

    #endregion Fields

    public MediaViewModel(HostViewModel hostViewModel)
    {
        PreLoop = new AudioViewModel(Audio.PreLoop);

        Opening = new AudioViewModel(Audio.Opening);
        Opening.PlayAudioCommand.Do(_ => hostViewModel.IsLogoShown = true)
                                .Subscribe();

        Meeting = new AudioViewModel(Audio.Meeting);
        BackgroundGeneral = new AudioViewModel(Audio.BackgroundGeneral);

        RulesExplanation = new AudioViewModel();
        RulesExplanation.PlayAudioCommand.Do(_ => RulesExplanationState += 1)
                                         .Subscribe();

        this.WhenAnyValue(media => media.RulesExplanationState)
            .Do(state =>
            {
                switch (RulesExplanationState)
                {
                    case RulesExplanationState.Begin:
                        RulesExplanation.Description = "Демонстрация дерева";
                        AudioManager.Play(Audio.RulesExplanation);
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
                        AudioManager.Play(Audio.RulesEnd);
                        hostViewModel.IsLogoShown = false;
                        hostViewModel.BigMoneyTree.IsShown = false;
                        hostViewModel.BigMoneyTree.StopDemo();
                        RulesExplanationState = RulesExplanationState.None;
                        break;

                    default:
                        RulesExplanation.Description = "Правила игры";
                        break;
                }
            }).Subscribe();

        CommercialIn = new AudioViewModel(Audio.CommercialIn);
        CommercialIn.PlayAudioCommand.Do(_ => hostViewModel.IsLogoShown = true)
                                     .Subscribe();

        CommercialOut = new AudioViewModel(Audio.CommercialOut);
        CommercialOut.PlayAudioCommand.Do(_ =>
        {
            _commOutSubscription?.Dispose();
            _commOutSubscription = Observable.Timer(TimeSpan.FromMilliseconds(5000), RxApp.MainThreadScheduler)
                                             .Do(_ => hostViewModel.IsLogoShown = false)
                                             .Subscribe();
        }).Subscribe();

        Goodbye = new AudioViewModel(Audio.Goodbye);
        Goodbye.PlayAudioCommand.Do(_ =>
        {
            hostViewModel.ClearScreen();
            hostViewModel.Winnings.IsShown = true;
        }).Subscribe();

        FinalSiren = new AudioViewModel(Audio.FinalSiren);

        ClosingTitles = new AudioViewModel(Audio.Closing);
        ClosingTitles.PlayAudioCommand.Do(_ =>
        {
            _closingTitlesSubscription?.Dispose();
            _closingTitlesSubscription = Observable.Timer(TimeSpan.FromMilliseconds(80000), RxApp.MainThreadScheduler)
                                                   .Do(_ => hostViewModel.IsLogoShown = true)
                                                   .Subscribe();
        }).Subscribe();
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