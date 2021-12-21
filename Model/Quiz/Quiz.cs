using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace WwtbamOld.Model;

[XmlType(TypeName = "quiz")]
public sealed class Quiz : PropertyChangedBase
{
    #region Fields

    private string _theme;
    private byte _level;
    private string _question;
    private string _photo;
    private string _comment;

    #endregion Fields

    public Quiz()
    {
    }

    public Quiz(string theme, byte level, string question, string photo, string a, string b, string c, string d, AnswerID correct, AnswerID alternative, string comment)
    {
        Theme = theme;
        Level = level;
        Question = question;
        Photo = photo;
        Answers = new Answers(a, b, c, d) { Correct = correct, Alternative = alternative };
        Comment = comment;
    }

    #region Properties

    [XmlAttribute(AttributeName = "theme")]
    [JsonPropertyName("theme")]
    public string Theme { get => _theme; set => RaiseAndSetIfChanged(ref _theme, value); }

    [XmlAttribute(AttributeName = "level")]
    [JsonPropertyName("level")]
    public byte Level { get => _level; set => RaiseAndSetIfChanged(ref _level, value); }

    [XmlElement(ElementName = "question")]
    [JsonPropertyName("question")]
    public string Question { get => _question; set => RaiseAndSetIfChanged(ref _question, value); }

    [XmlElement(ElementName = "photo")]
    [JsonPropertyName("photo")]
    public string Photo { get => _photo; set => RaiseAndSetIfChanged(ref _photo, value); }

    [XmlElement(ElementName = "answers")]
    [JsonPropertyName("answers")]
    public Answers Answers { get; init; }

    [XmlElement(ElementName = "comment")]
    [JsonPropertyName("comment")]
    public string Comment { get => _comment; set => RaiseAndSetIfChanged(ref _comment, value); }

    public static Quiz Default { get; } =
            new Quiz(theme: "Тема вопроса",
                     level: 1,
                     question: "Текст вопроса",
                     photo: null,
                     a: "Вариант A",
                     b: "Вариант B",
                     c: "Вариант C",
                     d: "Вариант D",
                     correct: AnswerID.A,
                     alternative: AnswerID.B,
                     comment: "Комментарий.");

    #endregion Properties

    public override string ToString() => $"{Level}. {Question} {Answers}";
}