using System.Xml;
using System.Xml.Serialization;

namespace WwtbamOld.Model;

[XmlType(TypeName = "quiz")]
public sealed class QuizInfo : PropertyChangedBase
{
    private string _theme;
    private byte _level;
    private string _question;
    private string _photo;
    private string _comment;

    [XmlAttribute(AttributeName = "theme")]
    public string Theme { get => _theme; set => RaiseAndSetIfChanged(ref _theme, value); }

    [XmlAttribute(AttributeName = "level")]
    public byte Level { get => _level; set => RaiseAndSetIfChanged(ref _level, value); }

    [XmlElement(ElementName = "question")]
    public string Question { get => _question; set => RaiseAndSetIfChanged(ref _question, value); }

    [XmlElement(ElementName = "photo")]
    public string Photo { get => _photo; set => RaiseAndSetIfChanged(ref _photo, value); }

    [XmlElement(ElementName = "answers")]
    public AnswersInfo Answers { get; init; }

    [XmlElement(ElementName = "comment")]
    public string Comment { get => _comment; set => RaiseAndSetIfChanged(ref _comment, value); }

    public QuizInfo()
    {
    }

    public QuizInfo(string theme, byte level, string question, string photo, string a, string b, string c, string d, AnswerID correct, AnswerID alternative, string comment)
    {
        Theme = theme;
        Level = level;
        Question = question;
        Photo = photo;
        Answers = new AnswersInfo(a, b, c, d) { Correct = correct, Alternative = alternative };
        Comment = comment;
    }

    public static QuizInfo Default { get; } =
            new QuizInfo("Тема вопроса", 1, "Текст вопроса", null, "Вариант A", "Вариант B", "Вариант C", "Вариант D", AnswerID.A, AnswerID.B, "Комментарий.");

    public override string ToString() => string.Join(" | ", Question, Answers.A, Answers.B, Answers.C, Answers.D);
}