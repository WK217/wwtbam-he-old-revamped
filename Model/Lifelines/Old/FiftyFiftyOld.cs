using ReactiveUI;
using System.Reactive;
using System.Timers;
using WwtbamOld.Media.Audio;
using WwtbamOld.ViewModel;

namespace WwtbamOld.Model.Old
{
    public sealed class FiftyFiftyOld : LifelineOld
    {
        public override byte ID => 1;
        public override string Code => "fifty";
        public override string Name => "«50 на 50»";
        public override Audio PingSound => Audio.PingFifty;

        public FiftyFiftyOld(HostViewModel hostViewModel)
        {
            _hostViewModel = hostViewModel;
            ActivateCommand = ReactiveCommand.Create(() =>
            {
                foreach (Answer answer in _hostViewModel.CurrentQuiz)
                {
                    //answer.Shown = answer.IsCorrect || answer == Alternative;
                }

                Alternative = null;
                Timer pingTimer = new Timer(150.0);

                pingTimer.Elapsed += (s, e) =>
                {
                    Enabled = false;
                    pingTimer.Stop();
                };

                State = LifelineState.Activated;
                AudioManager.Play(Audio.FiftyUse);
                pingTimer.Start();
            }, this.WhenAnyValue(fifty => fifty.Enabled, fifty => fifty.Alternative,
                                (enabled, alternative) => enabled && alternative != null && !alternative.IsCorrect));
            //).Select(x => x.Item1 && x.Item2 != null && !x.Item2.IsCorrect));
        }

        public AnswerViewModel Alternative { get; set; }

        private readonly HostViewModel _hostViewModel;
        public override ReactiveCommand<Unit, Unit> ActivateCommand { get; }
    }
}