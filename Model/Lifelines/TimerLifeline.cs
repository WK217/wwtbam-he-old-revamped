using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

public abstract class TimerLifeline : Lifeline
{
    #region Fields

    protected readonly TimeSpan _defaultDuration = TimeSpan.FromSeconds(30);
    protected readonly TimeSpan _interval = TimeSpan.FromMilliseconds(1000);

    private ObservableAsPropertyHelper<TimeSpan> _remainingTime;
    private readonly ObservableAsPropertyHelper<float> _progress;
    private IObservable<TimeSpan> _timerObservable;
    private IDisposable _timerSubscription;

    private static readonly TimeSpan _hideDelay = TimeSpan.FromMilliseconds(2500);
    private IDisposable _hideSubscription;

    #endregion Fields

    public event EventHandler<TimerLifelineStoppedEventArgs> TimerStopped;

    public TimerLifeline(Game game)
        : base(game)
    {
        _progress = this.WhenAnyValue(timer => timer.RemainingTime)
                        .Select(time => (float)(time / Duration * 100))
                        .ToProperty(this, nameof(Progress));

        Duration = _defaultDuration;
    }

    public TimerLifeline(Game game, TimeSpan duration)
        : this(game)
    {
        if (duration > TimeSpan.Zero)
        {
            _defaultDuration = duration;
            Duration = _defaultDuration;
        }
    }

    #region Properties

    [Reactive] public bool IsTimerShown { get; set; }
    [Reactive] public bool IsTimerActive { get; set; }

    [Reactive] public TimeSpan Duration { get; set; }
    public TimeSpan RemainingTime => _remainingTime is not null ? _remainingTime.Value : TimeSpan.Zero;

    public float Progress => _progress is not null ? _progress.Value : 0;

    protected abstract Audio AudioUse { get; }
    protected abstract Audio AudioStart { get; }
    protected abstract Audio AudioStop { get; }

    #endregion Properties

    #region Methods

    private void Execute(bool execute = true)
    {
        IsExecuting = execute;
        IsTimerShown = execute;
        State = execute ? LifelineState.Activated : LifelineState.Disabled;
    }

    public void Initiate()
    {
        Execute();
        AudioManager.Play(AudioUse);

        _remainingTime = Observable.Return(Duration)
                                   .ToProperty(this, nameof(RemainingTime), initialValue: Duration);
    }

    public override void Activate()
    {
        Execute();
        IsTimerActive = true;
        AudioManager.Play(AudioStart);

        _timerObservable = Observable.Interval(_interval, RxApp.MainThreadScheduler)
                                     .Select(x => Duration.Subtract(_interval.Multiply(x + 1)))
                                     .TakeWhile(x => x >= TimeSpan.Zero && IsTimerActive)
                                     .StartWith(Duration);

        _remainingTime = _timerObservable.ToProperty(this, nameof(RemainingTime));

        _timerSubscription = _timerObservable.Where(v => v == TimeSpan.Zero || !IsTimerActive)
                                             .Subscribe(x => Stop(ahead: false));

        _hideSubscription?.Dispose();
    }

    public void Stop(bool ahead = true)
    {
        IsTimerActive = false;

        _hideSubscription = Observable.Timer(_hideDelay, RxApp.MainThreadScheduler)
                                      .Subscribe(x =>
                                      {
                                          End();
                                          AudioManager.Instance.PlayBackground();
                                      });

        _timerSubscription?.Dispose();

        if (TimerStopped is not null)
            TimerStopped(this, new TimerLifelineStoppedEventArgs(ahead));
    }

    public void End()
    {
        Execute(false);
        IsTimerActive = false;
    }

    #endregion Methods
}
