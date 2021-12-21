using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace WwtbamOld.Model;

[XmlRoot(ElementName = "answers")]
public sealed class Answers : PropertyChangedBase
{
    #region Fields

    private string _a;
    private string _b;
    private string _c;
    private string _d;
    private AnswerID _correct;
    private AnswerID _alternative;

    #endregion Fields

    public Answers()
    {
    }

    public Answers(string a, string b, string c, string d)
    {
        (A, B, C, D) = (a, b, c, d);
    }

    #region Properties

    [XmlElement(ElementName = "a")]
    [JsonPropertyName("a")]
    public string A { get => _a; set => RaiseAndSetIfChanged(ref _a, value); }

    [XmlElement(ElementName = "b")]
    [JsonPropertyName("b")]
    public string B { get => _b; set => RaiseAndSetIfChanged(ref _b, value); }

    [XmlElement(ElementName = "c")]
    [JsonPropertyName("c")]
    public string C { get => _c; set => RaiseAndSetIfChanged(ref _c, value); }

    [XmlElement(ElementName = "d")]
    [JsonPropertyName("d")]
    public string D { get => _d; set => RaiseAndSetIfChanged(ref _d, value); }

    [XmlAttribute(AttributeName = "correct")]
    [JsonPropertyName("correct")]
    [Reactive] public AnswerID Correct { get => _correct; set => RaiseAndSetIfChanged(ref _correct, value); }

    [XmlAttribute(AttributeName = "fifty")]
    [JsonPropertyName("fifty")]
    [Reactive] public AnswerID Alternative { get => _alternative; set => RaiseAndSetIfChanged(ref _alternative, value); }

    #endregion Properties

    public string this[AnswerID id]
    {
        get
        {
            return id switch
            {
                AnswerID.A => A,
                AnswerID.B => B,
                AnswerID.C => C,
                AnswerID.D => D,
                _ => null
            };
        }

        set
        {
            switch (id)
            {
                case AnswerID.A:
                    A = value;
                    break;

                case AnswerID.B:
                    B = value;
                    break;

                case AnswerID.C:
                    C = value;
                    break;

                case AnswerID.D:
                    D = value;
                    break;

                default:
                    break;
            }
        }
    }

    public override string ToString() => $"A: {A}. B: {B}. C: {C}. D: {D}.";
}