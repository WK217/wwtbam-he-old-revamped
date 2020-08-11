using ReactiveUI;
using System;
using System.ComponentModel;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    [Description("«Двойной ответ»")]
    public sealed class DoubleDip : Lifeline
    {
        #region Basic Properties

        public override byte ID => 4;
        public override string Code => "double";
        public override string Name => "«Двойной ответ»";
        public override Audio PingSound => Audio.PingDoubleDip;

        #endregion Basic Properties

        #region Fields

        private DoubleDipMode _mode;

        #endregion Fields

        public DoubleDip(Game game) : base(game, true)
        {
        }

        #region Properties

        public DoubleDipMode Mode
        {
            get => _mode;
            set
            {
                var oldValue = _mode;
                this.RaiseAndSetIfChanged(ref _mode, value);

                if (_mode != oldValue)
                {
                    this.RaisePropertyChanged(nameof(IsActivated));

                    if (_mode == DoubleDipMode.Deactivated)
                    {
                        IsEnabled = false;
                        State = LifelineState.Disabled;
                    }
                }
            }
        }

        public Audio FinalAnswerAudio => Mode == DoubleDipMode.FirstAnswer ? Audio.DDLock1
                                       : Mode == DoubleDipMode.SecondAnswer ? Audio.DDLock2
                                       : AudioManager.Instance.FinalAnswerAudio;

        public override bool IsEnabled
        {
            get => base.IsEnabled;
            set
            {
                if (IsEnabled != value)
                {
                    base.IsEnabled = value;
                    if (!value && Mode != DoubleDipMode.Deactivated)
                        Deactivate();

                    this.RaisePropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public bool IsActivated => Mode > DoubleDipMode.Deactivated;

        public IObservable<bool> CanDeactivate => this.WhenAnyValue(dd => dd.IsActivated);

        #endregion Properties

        #region Methods

        private void Execute(bool execute = true)
        {
            State = execute ? LifelineState.Activated : LifelineState.Disabled;
            IsExecuting = execute;
        }

        public override void Activate()
        {
            Mode = DoubleDipMode.FirstAnswer;
            Execute();
            AudioManager.Instance.Play(Audio.DDUse);
        }

        public void SetMode(DoubleDipMode mode)
        {
            Execute();
            Mode = mode;
        }

        public void Deactivate()
        {
            Execute(false);
            _game.Lozenge.Locked = AnswerID.None;
            Mode = DoubleDipMode.Deactivated;
            AudioManager.Instance.PlayBackground();
        }

        #endregion Methods
    }
}