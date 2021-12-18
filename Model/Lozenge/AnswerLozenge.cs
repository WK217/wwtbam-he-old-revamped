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

    private readonly ObservableAsPropertyHelper<string> _text;
    private readonly ObservableAsPropertyHelper<bool> _isLocked;

    #endregion Fields

    public AnswerLozenge(Game game, Lozenge lozenge, AnswerID answerID)
    {
        _game = game;
        _lozenge = lozenge;
        ID = answerID;

        if (ID == AnswerID.A)
            this.WhenAnyValue(answer => answer._game.CurrentQuiz.A.Text).ToProperty(this, nameof(Text), out _text);
        else if (ID == AnswerID.B)
            this.WhenAnyValue(answer => answer._game.CurrentQuiz.B.Text).ToProperty(this, nameof(Text), out _text);
        else if (ID == AnswerID.C)
            this.WhenAnyValue(answer => answer._game.CurrentQuiz.C.Text).ToProperty(this, nameof(Text), out _text);
        else if (ID == AnswerID.D)
            this.WhenAnyValue(answer => answer._game.CurrentQuiz.D.Text).ToProperty(this, nameof(Text), out _text);

        _isLocked = this.WhenAnyValue(answer => answer._lozenge.Locked)
                        .Select(locked => locked == ID)
                        .ToProperty(this, nameof(IsLocked));
    }

    #region Properties

    public AnswerID ID { get; }
    [Reactive] public bool IsShown { get; set; }
    public string Text => _text is not null ? _text.Value : string.Empty;
    public bool IsLocked => _isLocked is not null && _isLocked.Value;

    [Reactive] public RevealCorrectType RevealCorrectType { get; set; }

    #endregion Properties

    #region Methods

    public void RevealCorrect(bool walkaway)
    {
        RevealCorrectType = _game.CurrentQuiz.Correct == ID
            ? ((_game.CurrentLevel.Number == 5 && IsLocked) || _game.CurrentLevel.Number >= 6) ?
                (IsLocked ? RevealCorrectType.Medium : RevealCorrectType.Slow) :
                RevealCorrectType.Quick
            : RevealCorrectType.None;
    }

    #endregion Methods

    public override string ToString() => $"{ID}: {Text}";
}
