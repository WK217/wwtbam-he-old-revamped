using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class Quizbase : ReadOnlyReactiveCollection<QuizInfo>
{
    public Quizbase(Game game) : base()
    {
        Initialize(ResourceManager.LoadQuizzesDefault());
        this.WhenAnyValue(qb => qb.SelectedQuiz)
            .Where(q => q is not null)
            .BindTo(game, game => game.CurrentQuiz.Info);
    }

    #region Properties

    [Reactive] public QuizInfo SelectedQuiz { get; set; }

    #endregion Properties

    #region Methods

    public void Initialize(IEnumerable<QuizInfo> collection)
    {
        _collection.Clear();
        _collection.AddRange(collection);

        if (_collection.Count > 0)
            SelectedQuiz = _collection[0];
    }

    public void Initialize(QuizInfo quiz)
    {
        _collection.Clear();
        _collection.Add(quiz);

        SelectedQuiz = quiz;
    }

    public void Initialize(params QuizInfo[] quizzes) => Initialize(quizzes);

    #endregion Methods
}