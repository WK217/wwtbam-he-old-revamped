using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class LozengeViewModel : ReactiveObject
{
    #region Fields

    private readonly ObservableAsPropertyHelper<Lifeline> _activatedLifeline;

    #endregion Fields

    public LozengeViewModel(Lozenge lozenge)
    {
        Model = lozenge;

        _activatedLifeline = this.WhenAnyValue(vm => vm.Model.ActivatedLifeline).ToProperty(this, nameof(ActivatedLifeline));
        this.WhenAnyValue(lvm => lvm.Model.IsShown).BindTo(this, lvm => lvm.IsShown);
        this.WhenAnyValue(lvm => lvm.IsShown).BindTo(this, lvm => lvm.Model.IsShown);

        ShowLifelinesPanelCommand = ReactiveCommand.Create(() => { LifelinesPanel.IsShown = !LifelinesPanel.IsShown; });
        RevealCorrectCommand = ReactiveCommand.Create(() => { return Model.RevealCorrect(); });
    }

    #region Properties

    public Lozenge Model { get; }

    [Reactive] public bool IsShown { get; set; }

    public AnswerLozenge A => Model.A;
    public AnswerLozenge B => Model.B;
    public AnswerLozenge C => Model.C;
    public AnswerLozenge D => Model.D;

    public AnswerLozenge this[AnswerID id]
    {
        get
        {
            return id switch
            {
                AnswerID.A => A,
                AnswerID.B => B,
                AnswerID.C => C,
                AnswerID.D => D,
                _ => null,
            };
        }
    }

    public LifelinesPanel LifelinesPanel => Model.LifelinesPanel;

    public Lifeline ActivatedLifeline => _activatedLifeline.Value;

    #endregion Properties

    #region Commands

    public ReactiveCommand<Unit, Unit> ShowLifelinesPanelCommand { get; }
    public ReactiveCommand<Unit, RevealCorrectType> RevealCorrectCommand { get; }

    #endregion Commands
}