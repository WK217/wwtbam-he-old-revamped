using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace WwtbamOld.Model;

public sealed class Answer : ReactiveObject
{
    #region Fields

    private readonly Quiz _quiz;

    #endregion Fields

    public Answer(Quiz quiz, AnswerID id)
    {
        _quiz = quiz;
        ID = id;
    }

    #region Properties

    public AnswerID ID { get; }
    [Reactive] public string Text { get; set; }

    public bool IsCorrect { get => _quiz.Correct == ID; set { if (value) _quiz.Correct = ID; } }

    #endregion Properties

    public override string ToString() => $"{ID}: {Text}";
}