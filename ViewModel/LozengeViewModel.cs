using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class LozengeViewModel : ReactiveObject
    {
        #region Fields

        private readonly Lozenge _lozenge;
        private readonly ObservableAsPropertyHelper<string> _questionText;
        private readonly ObservableAsPropertyHelper<Lifeline> _activatedLifeline;

        #endregion Fields

        public LozengeViewModel(Lozenge lozenge)
        {
            _lozenge = lozenge;

            _questionText = this.WhenAnyValue(vm => vm._lozenge.QuestionText).ToProperty(this, nameof(QuestionText));
            _activatedLifeline = this.WhenAnyValue(vm => vm._lozenge.ActivatedLifeline).ToProperty(this, nameof(ActivatedLifeline));
            this.WhenAnyValue(lvm => lvm._lozenge.IsShown).BindTo(this, lvm => lvm.IsShown);
            this.WhenAnyValue(lvm => lvm.IsShown).BindTo(this, lvm => lvm._lozenge.IsShown);

            ShowLifelinesPanelCommand = ReactiveCommand.Create(() => { LifelinesPanel.IsShown = !LifelinesPanel.IsShown; });
            RevealCorrectCommand = ReactiveCommand.Create(() => { return _lozenge.RevealCorrect(); });
        }

        #region Properties

        [Reactive] public bool IsShown { get; set; }

        public string QuestionText => _questionText is not null ? _questionText.Value : string.Empty;

        public AnswerLozenge A => _lozenge.A;
        public AnswerLozenge B => _lozenge.B;
        public AnswerLozenge C => _lozenge.C;
        public AnswerLozenge D => _lozenge.D;

        public AnswerLozenge this[AnswerID id]
        {
            get
            {
                return id switch
                {
                    AnswerID.B => B,
                    AnswerID.C => C,
                    AnswerID.D => D,
                    _ => A,
                };
            }
        }

        public LifelinesPanel LifelinesPanel => _lozenge.LifelinesPanel;

        public Lifeline ActivatedLifeline => _activatedLifeline.Value;

        #endregion Properties

        #region Commands

        public ReactiveCommand<Unit, Unit> ShowLifelinesPanelCommand { get; }
        public ReactiveCommand<Unit, RevealCorrectType> RevealCorrectCommand { get; }

        #endregion Commands
    }
}