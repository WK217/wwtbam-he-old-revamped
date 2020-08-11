using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class PhoneAFriendViewModel : LifelineViewModel<PhoneAFriend>
    {
        #region Fields

        private readonly ObservableAsPropertyHelper<uint> _countdown;

        #endregion Fields

        public PhoneAFriendViewModel(LifelinesViewModel lifelinesViewModel, PhoneAFriend lifeline) : base(lifelinesViewModel, lifeline)
        {
            this.WhenAnyValue(vm => vm._model.Duration).Select(x => (uint)x.TotalSeconds).BindTo(this, x => x.Duration);
            this.WhenAnyValue(vm => vm.Duration).Select(x => TimeSpan.FromSeconds(x)).BindTo(_model, x => x.Duration);

            this.WhenAnyValue(vm => vm._model.IsTimerShown).BindTo(this, x => x.IsTimerShown);
            this.WhenAnyValue(vm => vm.IsTimerShown).BindTo(_model, x => x.IsTimerShown);

            _countdown = this.WhenAnyValue(vm => vm._model.RemainingTime).Select(x => (uint)x.TotalSeconds).ToProperty(this, nameof(Countdown));

            InitiateCommand = ReactiveCommand.Create(() => _model.Initiate());
            StopCommand = ReactiveCommand.Create(() => _model.Stop());
            EndCommand = ReactiveCommand.Create(() => _model.End());
        }

        #region Properties

        [Reactive] public uint Duration { get; set; }
        public uint Countdown => _countdown != null ? _countdown.Value : 0;

        [Reactive] public bool IsTimerShown { get; set; }

        #endregion Properties

        #region Commands

        public ReactiveCommand<Unit, Unit> InitiateCommand { get; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; }
        public ReactiveCommand<Unit, Unit> EndCommand { get; }

        #endregion Commands
    }
}