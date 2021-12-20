using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class Lozenge : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly ObservableAsPropertyHelper<Lifeline> _activatedLifeline;

    private Quiz _info;

    private static readonly List<string> _propertiesToUpdate = new(4)
    {
        nameof(Level),
        nameof(Theme),
        nameof(Question),
        nameof(Photo),
        nameof(Correct),
        nameof(Alternative),
        nameof(Comment),
    };

    #endregion Fields

    public Lozenge(Game game)
    {
        _game = game;

        A = new AnswerLozenge(_game, this, AnswerID.A);
        B = new AnswerLozenge(_game, this, AnswerID.B);
        C = new AnswerLozenge(_game, this, AnswerID.C);
        D = new AnswerLozenge(_game, this, AnswerID.D);

        A.PropertyChanged += OnAnswerPropertyChanged;
        B.PropertyChanged += OnAnswerPropertyChanged;
        C.PropertyChanged += OnAnswerPropertyChanged;
        D.PropertyChanged += OnAnswerPropertyChanged;

        Info = Quiz.Default;

        this.WhenAnyValue(quiz => quiz.Correct).Subscribe(id =>
        {
            string propertyName = nameof(AnswerLozenge.IsCorrect);
            A.RaisePropertyChanged(propertyName);
            B.RaisePropertyChanged(propertyName);
            C.RaisePropertyChanged(propertyName);
            D.RaisePropertyChanged(propertyName);
        });

        this.WhenAnyValue(lozenge => lozenge.Info).Subscribe(q =>
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

    public Quiz Info
    {
        get => _info;
        set
        {
            if (_info != value)
            {
                if (_info is not null)
                {
                    _info.PropertyChanged -= OnQuizInfoPropertyChanged;
                    _info.Answers.PropertyChanged -= OnAnswersPropertyChanged;
                }

                this.RaiseAndSetIfChanged(ref _info, value ?? Quiz.Default);

                _info.PropertyChanged += OnQuizInfoPropertyChanged;
                _info.Answers.PropertyChanged += OnAnswersPropertyChanged;

                foreach (string propertyName in _propertiesToUpdate)
                    this.RaisePropertyChanged(propertyName);

                A.Text = _info.Answers.A;
                B.Text = _info.Answers.B;
                C.Text = _info.Answers.C;
                D.Text = _info.Answers.D;

                Correct = _info.Answers.Correct;
                this.RaisePropertyChanged(nameof(Correct));
            }
        }
    }

    public byte Level { get => _info.Level; set { _info.Level = value; this.RaisePropertyChanged(); } }
    public string Theme { get => _info.Theme; set { _info.Theme = value; this.RaisePropertyChanged(); } }

    public string Question { get => _info.Question; set { _info.Question = value; this.RaisePropertyChanged(); } }
    public string Photo { get => _info.Photo; set { _info.Photo = value; this.RaisePropertyChanged(); } }

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
                AnswerID.A => A,
                AnswerID.B => B,
                AnswerID.C => C,
                AnswerID.D => D,
                _ => null,
            };
        }
    }

    public AnswerID Correct { get => _info.Answers.Correct; set { _info.Answers.Correct = value; this.RaisePropertyChanged(); } }
    public AnswerID Alternative { get => _info.Answers.Alternative; set { _info.Answers.Alternative = value; this.RaisePropertyChanged(); } }

    public string Comment { get => _info.Comment; set { _info.Comment = value; this.RaisePropertyChanged(); } }

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
        var correct = this[_game.Lozenge.Correct];
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

    private void OnQuizInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_propertiesToUpdate.Contains(e.PropertyName))
            this.RaisePropertyChanged(e.PropertyName);
    }

    private void OnAnswersPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Model.Answers.Correct))
            this.RaisePropertyChanged(nameof(Correct));
    }

    private void OnAnswerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is IAnswer answer)
        {
            if (e.PropertyName == nameof(IAnswer.Text))
                _info.Answers[answer.ID] = answer.Text;
        }
    }

    public override string ToString() => string.Join(" | ", Question, A, B, C, D);

    #endregion Methods
}