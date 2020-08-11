using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    [Description("«Звонок другу»")]
    public sealed class PhoneAFriend : Lifeline
    {
        #region Basic Properties

        public override byte ID => 2;
        public override string Code => "phone";
        public override string Name => "«Звонок другу»";
        public override Audio PingSound => Audio.PingPhone;

        #endregion Basic Properties

        #region Fields

        private ObservableAsPropertyHelper<TimeSpan> _remainingTime;
        private ObservableAsPropertyHelper<float> _progress;

        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(1000);
        private IObservable<TimeSpan> _timerObservable;
        private IDisposable _timerSubscription;

        private TimeSpan _delayHide = TimeSpan.FromMilliseconds(2500);
        private IDisposable _timerHideSubscription;

        #endregion Fields

        public PhoneAFriend(Game game) : base(game)
        {
            _progress = this.WhenAnyValue(phone => phone.RemainingTime)
                            .Select(v => (float)(v / Duration * 100))
                            .ToProperty(this, nameof(Progress));
        }

        #region Properties

        [Reactive] public bool IsTimerShown { get; set; }
        [Reactive] public bool IsTimerActive { get; set; }

        [Reactive] public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan RemainingTime => _remainingTime != null ? _remainingTime.Value : TimeSpan.Zero;

        public float Progress => _progress != null ? _progress.Value : 0;

        bool IsDefaultDuration => Duration == TimeSpan.FromSeconds(30);
        Audio AudioUse => IsDefaultDuration ? Audio.PhoneUse : Audio.TWMUse;
        Audio AudioStart => IsDefaultDuration ? Audio.PhoneStart : Audio.TWMStart;
        Audio AudioStop => IsDefaultDuration ? Audio.PhoneStop : Audio.TWMStop;

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
            AudioManager.Instance.Play(AudioUse);

            Observable.Return(Duration)
                      .ToProperty(this, nameof(RemainingTime), out _remainingTime, initialValue: Duration);
        }

        public override void Activate()
        {
            Execute();
            IsTimerActive = true;
            AudioManager.Instance.Play(AudioStart);

            _timerObservable = Observable.Interval(_interval, RxApp.MainThreadScheduler)
                                         .Select(x => Duration.Subtract(_interval.Multiply(x + 1)))
                                         .TakeWhile(x => x >= TimeSpan.Zero && IsTimerActive)
                                         .StartWith(Duration);

            _timerObservable.ToProperty(this, nameof(RemainingTime), out _remainingTime);

            _timerSubscription = _timerObservable.Where(v => v == TimeSpan.Zero || !IsTimerActive)
                                                 .Subscribe(x => Stop(!IsDefaultDuration));
        }

        public void Stop(bool ahead = true)
        {
            IsTimerActive = false;

            _timerHideSubscription = Observable.Timer(_delayHide, RxApp.MainThreadScheduler)
                                               .Subscribe(x =>
                                               {
                                                   End();
                                                   AudioManager.Instance.PlayBackground();
                                               });

            if (ahead)
            {
                _timerSubscription?.Dispose();
                AudioManager.Instance.Play(AudioStop);
            }
        }

        public void End()
        {
            Execute(false);
            IsTimerActive = false;
        }

        #endregion Methods
    }
}