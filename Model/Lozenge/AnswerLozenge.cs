using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using WwtbamOld.ViewModel;

namespace WwtbamOld.Model;

public sealed class AnswerLozenge : ReactiveObject, IAnswerLozengeViewModel
{
    #region Fields

    private readonly Game _game;
    private readonly Lozenge _lozenge;

    private readonly ObservableAsPropertyHelper<bool> _isLocked;

    #endregion Fields

    public AnswerLozenge(Game game, Lozenge lozenge, AnswerID answerID)
    {
        _game = game;
        _lozenge = lozenge;
        ID = answerID;

        _isLocked = this.WhenAnyValue(answer => answer._lozenge.Locked)
                        .Select(locked => locked == ID)
                        .ToProperty(this, nameof(IsLocked));
    }

    #region Properties

    public AnswerID ID { get; }
    [Reactive] public bool IsShown { get; set; }
    [Reactive] public string Text { get; set; }

    public bool IsCorrect { get => _lozenge.Correct == ID; set { if (value) _lozenge.Correct = ID; } }
    public bool IsLocked => _isLocked is not null && _isLocked.Value;

    [Reactive] public RevealCorrectType RevealCorrectType { get; set; }

    #endregion Properties

    #region Methods

    public void RevealCorrect(bool walkaway)
    {
        RevealCorrectType = _lozenge.Correct == ID
            ? ((_game.CurrentLevel.Number == 5 && IsLocked) || _game.CurrentLevel.Number >= 6) ?
                (IsLocked ? RevealCorrectType.Medium : RevealCorrectType.Slow) :
                RevealCorrectType.Quick
            : RevealCorrectType.None;
    }

    public override string ToString() => $"{ID}: {Text}";

    #endregion Methods
}