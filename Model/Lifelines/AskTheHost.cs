using System;
using System.ComponentModel;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    [Description("«Помощь ведущего»")]
    public sealed class AskTheHost : Lifeline
    {
        #region Basic Properties

        public override byte ID => 5;
        public override string Code => "ath";
        public override string Name => "«Помощь ведущего»";
        public override Audio PingSound => Audio.PingDoubleDip;

        #endregion Basic Properties

        #region Fields

        private IDisposable _timerBackgroundMusicSubscription;

        #endregion Fields

        public AskTheHost(Game game) : base(game, true)
        {
        }

        #region Methods

        private void Execute(bool execute = true)
        {
            IsExecuting = execute;
            State = execute ? LifelineState.Activated : LifelineState.Disabled;
        }

        public override void Activate()
        {
            Execute();
            AudioManager.Play(Audio.AtHStart);
        }

        public void Deactivate()
        {
            Execute(false);
            AudioManager.Play(Audio.AtHStop);

            _timerBackgroundMusicSubscription?.Dispose();
            _timerBackgroundMusicSubscription = Observable.Timer(TimeSpan.FromMilliseconds(4000))
                                                          .Subscribe(_ => AudioManager.Instance.PlayBackground());
        }

        #endregion Methods
    }
}