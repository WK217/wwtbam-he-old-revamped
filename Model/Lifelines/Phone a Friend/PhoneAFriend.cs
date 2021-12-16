using System.ComponentModel;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    [Description("«Звонок другу»")]
    public sealed class PhoneAFriend : TimerLifeline
    {
        #region Basic Properties

        public override byte ID => 2;
        public override string Code => "phone";
        public override string Name => "«Звонок другу»";

        #endregion Basic Properties

        public PhoneAFriend(Game game)
            : base(game)
        {
            TimerStopped += PhoneTimerStopped;
        }

        #region Properties

        private bool IsDefaultDuration => Duration == _defaultDuration;

        protected override Audio AudioUse => IsDefaultDuration ? Audio.PhoneUse : Audio.TWMUse;
        protected override Audio AudioStart => IsDefaultDuration ? Audio.PhoneStart : Audio.TWMCountdown;
        protected override Audio AudioStop => IsDefaultDuration ? Audio.PhoneStop : Audio.TWMStop;

        #endregion Properties

        #region Methods

        private void PhoneTimerStopped(object sender, TimerLifelineStoppedEventArgs e)
        {
            if (e.Ahead)
                AudioManager.Play(AudioStop);
        }

        public override void Activate()
        {
            base.Activate();

            if (!IsDefaultDuration)
                AudioManager.Play(Audio.TWMStart);
        }

        #endregion Methods
    }
}