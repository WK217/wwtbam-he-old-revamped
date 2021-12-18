using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class Lozenge : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly ObservableAsPropertyHelper<string> _questionText;
    private readonly ObservableAsPropertyHelper<Lifeline> _activatedLifeline;

    #endregion Fields

    public Lozenge(Game game)
    {
        _game = game;

        A = new AnswerLozenge(_game, this, AnswerID.A);
        B = new AnswerLozenge(_game, this, AnswerID.B);
        C = new AnswerLozenge(_game, this, AnswerID.C);
        D = new AnswerLozenge(_game, this, AnswerID.D);

        _questionText = this.WhenAnyValue(lozenge => lozenge._game.CurrentQuiz.Question.Text).ToProperty(this, nameof(QuestionText));
        this.WhenAnyValue(lozenge => lozenge._game.CurrentQuiz).Subscribe(q =>
        {
            Locked = AnswerID.None;

            A.RevealCorrectType = RevealCorrectType.None;
            B.RevealCorrectType = RevealCorrectType.None;
            C.RevealCorrectType = RevealCorrectType.None;
            D.RevealCorrectType = RevealCorrectType.None;
        });
        this.WhenAnyValue(lozenge => lozenge.Locked).Subscribe(v =>
        {
            A.RevealCorrectType = RevealCorrectType.None;
            B.RevealCorrectType = RevealCorrectType.None;
            C.RevealCorrectType = RevealCorrectType.None;
            D.RevealCorrectType = RevealCorrectType.None;
        });

        LifelinesPanel = new LifelinesPanel(_game.Lifelines);

        _activatedLifeline = this.WhenAnyValue(x => x._game.Lifelines.Selected).ToProperty(this, nameof(ActivatedLifeline));
    }

    #region Properties

    [Reactive] public bool IsShown { get; set; }

    public string QuestionText => _questionText is not null ? _questionText.Value : string.Empty;

    public AnswerLozenge A { get; }
    public AnswerLozenge B { get; }
    public AnswerLozenge C { get; }
    public AnswerLozenge D { get; }

    [Reactive] public AnswerID Locked { get; set; }

    public LifelinesPanel LifelinesPanel { get; }

    public IEnumerable<AnswerLozenge> Answers
    {
        get
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

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

    public Lifeline ActivatedLifeline => _activatedLifeline?.Value;

    #endregion Properties

    #region Methods

    public void ShowAnswers(bool show = true)
    {
        A.IsShown = show;
        B.IsShown = show;
        C.IsShown = show;
        D.IsShown = show;
    }

    public void LockAnswer(AnswerID answerID)
    {
        Locked = answerID;
    }

    public RevealCorrectType RevealCorrect(bool walkaway = false)
    {
        var correct = this[_game.CurrentQuiz.Correct];
        correct.RevealCorrect(walkaway);
        return correct.RevealCorrectType;
    }

    public void Clear()
    {
        IsShown = false;
        Locked = AnswerID.None;

        foreach (var answer in Answers)
        {
            answer.IsShown = false;
            answer.RevealCorrectType = RevealCorrectType.None;
        }
    }

    #endregion Methods
}
