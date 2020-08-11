using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Timers;
using System.Windows.Media;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model.Old
{
    public sealed class PhoneAFriendOld : LifelineOld
    {
        private readonly ReactiveCommand<Unit, Unit> _activateCommand;

        public override byte ID => 2;
        public override string Code => "phone";
        public override string Name => "«Помощь друга»";
        public override Audio PingSound => Audio.PingPhone;

        public PhoneAFriendTimerOld Timer { get; }

        public PhoneAFriendOld()
        {
            Timer = new PhoneAFriendTimerOld(this);
            Timer.CountdownCompleted += TimerCountdownCompleted;

            InitiateCommand = ReactiveCommand.Create(() =>
            {
                Shown = true;
                State = LifelineState.Activated;
                Timer.Percentage = 100.0;
                AudioManager.Instance.Play(Audio.TWMUse);
            });

            _activateCommand = ReactiveCommand.Create(() =>
            {
                Shown = true;
                State = LifelineState.Activated;
                Timer.Start();
                AudioManager.Instance.Play(Audio.TWMStart);
            });

            StopCommand = ReactiveCommand.Create(() =>
            {
                Timer.Stop();
                TimerCountdownCompleted(this, EventArgs.Empty);
            });
        }

        private void TimerCountdownCompleted(object sender, EventArgs eventArgs)
        {
            AudioManager.Instance.Play(Audio.TWMStop);

            Timer hideTimer = new Timer(4000.0);
            hideTimer.Elapsed += delegate (object s, ElapsedEventArgs e)
            {
                Shown = false;
                State = LifelineState.Disabled;
                AudioManager.Instance.PlayBackground();
                hideTimer.Stop();
            };

            hideTimer.Start();
        }

        [Reactive]
        public bool Shown { get; set; }

        public ImageSource ClockImage => FileManager.GetImageSource(string.Format("{0}Phone Timer\\{1} timer graph {2}.png", "Resources\\Graphics\\", "wwtbam", 0));

        public override ReactiveCommand<Unit, Unit> ActivateCommand => _activateCommand;
        public ReactiveCommand<Unit, Unit> InitiateCommand { get; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; }
    }
}