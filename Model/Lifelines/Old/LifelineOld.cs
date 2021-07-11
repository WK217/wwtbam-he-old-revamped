using ReactiveUI;
using System.Reactive;
using System.Timers;
using System.Windows.Media;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model.Old
{
    public abstract class LifelineOld : ReactiveObject
    {
        protected bool _secret = true;
        protected LifelineState _state;
        private bool _menuShown;

        public abstract byte ID { get; }
        public abstract string Code { get; }
        public abstract string Name { get; }
        public abstract Audio PingSound { get; }

        public LifelineOld()
        {
            Included = true;

            #region Commands

            ShowMenuCommand = ReactiveCommand.Create(() => { MenuShown = !MenuShown; }, this.WhenAnyValue(l => l.Enabled));

            PingCommand = ReactiveCommand.Create(() =>
            {
                Timer pingTimer = new Timer(300.0);

                pingTimer.Elapsed += delegate (object s, ElapsedEventArgs e)
                {
                    State = LifelineState.Enabled;
                    pingTimer.Stop();
                };

                Secret = false;
                State = LifelineState.Activated;
                AudioManager.Play(PingSound);
                pingTimer.Start();
            });

            #endregion Commands
        }

        public virtual bool Enabled
        {
            get => _state != LifelineState.Disabled;
            set
            {
                if (Enabled != value)
                {
                    State = value ? LifelineState.Enabled : LifelineState.Disabled;
                    this.RaisePropertyChanged(nameof(Enabled));
                }
            }
        }

        public bool Secret
        {
            get => _secret;
            set
            {
                if (_secret != value)
                {
                    _secret = value;
                    if (_secret)
                        _menuShown = false;

                    this.RaisePropertyChanged(nameof(Secret));
                }
            }
        }

        public LifelineState State
        {
            get => Secret ? LifelineState.Secret : _state;
            set
            {
                if (State != value)
                {
                    _state = value;
                    if (State == LifelineState.Disabled || State == LifelineState.Secret)
                        _menuShown = false;

                    this.RaisePropertyChanged(nameof(State));
                }
            }
        }

        public ImageSource SmallImage => GetImage(LifelineImageType.Small, State);
        public ImageSource Small2Image => GetImage(LifelineImageType.Small2, State);
        public ImageSource MediumImage => GetImage(LifelineImageType.Medium, State);

        public bool MenuShown
        {
            get => State != LifelineState.Disabled && State != LifelineState.Secret && _menuShown;
            set
            {
                if (MenuShown != value)
                {
                    _menuShown = (State == LifelineState.Enabled || State == LifelineState.Activated) && value;
                    this.RaisePropertyChanged(nameof(MenuShown));
                }
            }
        }

        public bool Included { get; set; }

        private ImageSource GetImage(LifelineImageType type, LifelineState state)
        {
            string format = "{0}Lifelines\\{1}\\{2} {3} {4} {5}.png";
            object[] args = new[]
            {
                "Resources\\Graphics\\",
                type.ToString(),
                "wwtbam",
                Code.ToLower(),
                state.ToString().ToLower(),
                type.ToString().ToLower()
            };

            if (state == LifelineState.Secret)
            {
                format = "{0}Lifelines\\{1}\\{2} secret {3}.png";
                args = new[]
                {
                    "Resources\\Graphics\\",
                    type.ToString(),
                    "wwtbam",
                    type.ToString().ToLower()
                };
            }

            return FileManager.GetImageSource(string.Format(format, args));
        }

        #region Commands

        public ReactiveCommand<Unit, Unit> PingCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowMenuCommand { get; }
        public abstract ReactiveCommand<Unit, Unit> ActivateCommand { get; }

        #endregion Commands

        public override string ToString() => Name;
    }
}