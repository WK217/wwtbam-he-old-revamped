using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    public sealed class TestLifeline : ReactiveObject
    {
        private static TimeSpan InitialTime = TimeSpan.FromSeconds(5);
        private static double MaxValue = 100;
        private TimeSpan _interval;

        private ObservableAsPropertyHelper<TimeSpan> _remainingTime;
        public TimeSpan RemainingTime => _remainingTime != null ? _remainingTime.Value : TimeSpan.Zero;

        private ObservableAsPropertyHelper<double> _count;
        public double Count => _count != null ? _count.Value : 0;

        public ReactiveCommand<Unit, double> StartCommand { get; }

        private ObservableAsPropertyHelper<bool> _isRunning;
        public bool IsRunning => _isRunning != null ? _isRunning.Value : false;

        public TestLifeline(TimeSpan interval)
        {
            _interval = interval;
            var canStart = this.WhenAny(x => x.IsRunning, propChanged => !propChanged.Value);
            StartCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                var timeSpan = TimeSpan.FromSeconds(30);
                var step1 = _interval.TotalMilliseconds / timeSpan.TotalMilliseconds;
                var increment = MaxValue * step1;

                AudioManager.Play(Audio.PhoneStart);

                return Observable.Interval(_interval, RxApp.MainThreadScheduler)
                  .TakeWhile(_ => Count < MaxValue)
                  .Select(_ => Math.Round(Math.Max(Math.Min(Count + increment, 100), 0), 4));
            }, canStart);

            StartCommand.IsExecuting
                .ToProperty(this, nameof(IsRunning), out _isRunning);
            StartCommand.StartWith(0)
                .ToProperty(this, nameof(Count), out _count);
        }
    }
}