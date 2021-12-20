using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WwtbamOld.Model;

public sealed class Quiz : ReactiveObject
{
    #region Fields

    private QuizInfo _info;

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

    public Quiz()
    {
        A = new Answer(this, AnswerID.A);
        B = new Answer(this, AnswerID.B);
        C = new Answer(this, AnswerID.C);
        D = new Answer(this, AnswerID.D);

        A.PropertyChanged += OnAnswerPropertyChanged;
        B.PropertyChanged += OnAnswerPropertyChanged;
        C.PropertyChanged += OnAnswerPropertyChanged;
        D.PropertyChanged += OnAnswerPropertyChanged;

        Info = QuizInfo.Default;

        this.WhenAnyValue(quiz => quiz.Correct).Subscribe(id =>
        {
            string propertyName = nameof(Answer.IsCorrect);
            A.RaisePropertyChanged(propertyName);
            B.RaisePropertyChanged(propertyName);
            C.RaisePropertyChanged(propertyName);
            D.RaisePropertyChanged(propertyName);
        });
    }

    #region Properties

    public QuizInfo Info
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

                this.RaiseAndSetIfChanged(ref _info, value ?? QuizInfo.Default);

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

    public Answer A { get; }
    public Answer B { get; }
    public Answer C { get; }
    public Answer D { get; }

    public IEnumerable<Answer> Answers
    {
        get
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    public Answer this[AnswerID id]
    {
        get
        {
            return id switch
            {
                AnswerID.D => D,
                AnswerID.C => C,
                AnswerID.B => B,
                AnswerID.A => A,
                _ => null
            };
        }
    }

    public AnswerID Correct { get => _info.Answers.Correct; set { _info.Answers.Correct = value; this.RaisePropertyChanged(); } }
    public AnswerID Alternative { get => _info.Answers.Alternative; set { _info.Answers.Alternative = value; this.RaisePropertyChanged(); } }

    public string Comment { get => _info.Comment; set { _info.Comment = value; this.RaisePropertyChanged(); } }

    #endregion Properties

    #region Methods

    private void OnQuizInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_propertiesToUpdate.Contains(e.PropertyName))
            this.RaisePropertyChanged(e.PropertyName);
    }

    private void OnAnswersPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AnswersInfo.Correct))
            this.RaisePropertyChanged(nameof(Correct));
    }

    private void OnAnswerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is Answer answer)
        {
            if (e.PropertyName == nameof(Answer.Text))
                _info.Answers[answer.ID] = answer.Text;
        }
    }

    public override string ToString() => string.Join(" | ", Question, A, B, C, D);

    #endregion Methods
}