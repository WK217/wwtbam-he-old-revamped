using ReactiveUI;
using System;
using System.Reactive;
using WwtbamOld.Media.Audio;
using WwtbamOld.ViewModel;

namespace WwtbamOld.Model.Old
{
    public sealed class DoubleDipOld : LifelineOld
    {
        public override byte ID => 4;
        public override string Code => "double";
        public override string Name => "«Двойной ответ»";
        public override Audio PingSound => Audio.PingDoubleDip;

        public DoubleDipOld(HostViewModel hostViewModel)
        {
            _hostViewModel = hostViewModel;

            _activateCommand = ReactiveCommand.Create(() =>
            {
                Mode = DoubleDipMode.FirstAnswer;
                AudioManager.Play(Audio.DDUse);
                State = LifelineState.Activated;
            });

            SetModeCommand = ReactiveCommand.Create<DoubleDipMode>(mode => { Mode = (DoubleDipMode)Convert.ToInt32(mode); });

            DeactivateCommand = ReactiveCommand.Create(() =>
            {
                Mode = DoubleDipMode.Deactivated;
                AudioManager.Play(AudioManager.Instance.BackgroundAudio);
                State = LifelineState.Disabled;
            }, this.WhenAnyValue(dd => dd.Activated));
        }

        public DoubleDipMode Mode
        {
            get => _mode;
            set
            {
                if (_mode != value)
                {
                    _mode = value;
                    this.RaisePropertyChanged(nameof(Activated));

                    switch (_mode)
                    {
                        case DoubleDipMode.Deactivated:
                            AudioManager.Instance.CurrentLevel = _hostViewModel.CurrentLevel;
                            Enabled = false;
                            State = LifelineState.Disabled;
                            return;

                        case DoubleDipMode.FirstAnswer:
                            AudioManager.Instance.BackgroundAudio = Audio.DDUseLoop;
                            AudioManager.Instance.FinalAnswerAudio = Audio.DDLock1;
                            AudioManager.Instance.WrongAudio = Audio.DDWrong;
                            return;

                        case DoubleDipMode.SecondAnswer:
                            AudioManager.Instance.BackgroundAudio = Audio.DDWrongLoop;
                            AudioManager.Instance.FinalAnswerAudio = Audio.DDLock2;
                            AudioManager.Instance.WrongAudio = _hostViewModel.CurrentLevel.WrongAudio;
                            this.RaisePropertyChanged(nameof(Mode));
                            break;

                        default:
                            return;
                    }
                }
            }
        }

        public override bool Enabled
        {
            get => Enabled;
            set
            {
                if (Enabled != value)
                {
                    Enabled = value;
                    if (!value && Mode != DoubleDipMode.Deactivated)
                        DeactivateCommand.Execute(Unit.Default);

                    this.RaisePropertyChanged(nameof(Enabled));
                }
            }
        }

        public bool Activated => Mode > DoubleDipMode.Deactivated;

        #region Commands

        public override ReactiveCommand<Unit, Unit> ActivateCommand => _activateCommand;
        public ReactiveCommand<DoubleDipMode, Unit> SetModeCommand { get; }
        public ReactiveCommand<Unit, Unit> DeactivateCommand { get; }

        #endregion Commands

        private readonly HostViewModel _hostViewModel;
        private DoubleDipMode _mode;
        private readonly ReactiveCommand<Unit, Unit> _activateCommand;
    }
}