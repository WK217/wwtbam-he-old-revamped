using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace WwtbamOld.Model;

public sealed class Answer : ReactiveObject
{
    #region Fields

    private readonly Quiz _quiz;

    #endregion Fields

    public Answer(Quiz quiz, AnswerID id, string text)
    {
        _quiz = quiz;
        ID = id;
        Text = text;

        this.WhenAnyValue(answer => answer.IsCorrect).Subscribe(f =>
        {
            if (f)
                _quiz.Correct = ID;
        });
    }

    #region Properties

    public AnswerID ID { get; }
    [Reactive] public string Text { get; set; }
    [Reactive] public bool IsCorrect { get; set; }

    #endregion Properties

    public override string ToString() => $"{ID}: {Text}";
}
