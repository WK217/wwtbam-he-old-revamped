using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public class TimerLifelineViewModel : LifelineViewModel<TimerLifeline>
{
    #region Fields

    private readonly ObservableAsPropertyHelper<uint> _countdown;

    #endregion Fields

    public TimerLifelineViewModel(LifelinesViewModel lifelinesViewModel, TimerLifeline lifeline)
        : base(lifelinesViewModel, lifeline)
    {
        this.WhenAnyValue(vm => vm._model.Duration).Select(x => (uint)x.TotalSeconds).BindTo(this, x => x.Duration);
        this.WhenAnyValue(vm => vm.Duration).Select(x => TimeSpan.FromSeconds(x)).BindTo(_model, x => x.Duration);

        this.WhenAnyValue(vm => vm._model.IsTimerShown).BindTo(this, x => x.IsTimerShown);
        this.WhenAnyValue(vm => vm.IsTimerShown).BindTo(_model, x => x.IsTimerShown);

        this.WhenAnyValue(vm => vm._model.Progress).Do(_ => this.RaisePropertyChanged(nameof(Progress))).Subscribe();
        this.WhenAnyValue(vm => vm._model.RemainingTime).Do(_ => this.RaisePropertyChanged(nameof(RemainingTime))).Subscribe();

        _countdown = this.WhenAnyValue(vm => vm._model.RemainingTime).Select(x => (uint)x.TotalSeconds).ToProperty(this, nameof(Countdown));

        InitiateCommand = ReactiveCommand.Create(() => _model.Initiate());
        StopCommand = ReactiveCommand.Create(() => _model.Stop());
        EndCommand = ReactiveCommand.Create(() => _model.End());
    }

    #region Properties

    [Reactive] public uint Duration { get; set; }
    public uint Countdown => _countdown is not null ? _countdown.Value : 0;

    [Reactive] public bool IsTimerShown { get; set; }

    public float Progress => _model.Progress;
    public TimeSpan RemainingTime => _model.RemainingTime;

    #endregion Properties

    #region Commands

    public ReactiveCommand<Unit, Unit> InitiateCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }
    public ReactiveCommand<Unit, Unit> EndCommand { get; }

    #endregion Commands
}