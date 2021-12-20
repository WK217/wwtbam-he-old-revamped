using ReactiveUI.Fody.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace WwtbamOld.Model;

[XmlRoot(ElementName = "answers")]
public sealed class AnswersInfo : PropertyChangedBase
{
    private string _a;
    private string _b;
    private string _c;
    private string _d;
    private AnswerID _correct;
    private AnswerID _alternative;

    [XmlElement(ElementName = "a")]
    public string A { get => _a; set => RaiseAndSetIfChanged(ref _a, value); }

    [XmlElement(ElementName = "b")]
    public string B { get => _b; set => RaiseAndSetIfChanged(ref _b, value); }

    [XmlElement(ElementName = "c")]
    public string C { get => _c; set => RaiseAndSetIfChanged(ref _c, value); }

    [XmlElement(ElementName = "d")]
    public string D { get => _d; set => RaiseAndSetIfChanged(ref _d, value); }

    [XmlAttribute(AttributeName = "correct")]
    [Reactive] public AnswerID Correct { get => _correct; set => RaiseAndSetIfChanged(ref _correct, value); }

    [XmlAttribute(AttributeName = "fifty")]
    [Reactive] public AnswerID Alternative { get => _alternative; set => RaiseAndSetIfChanged(ref _alternative, value); }

    public AnswersInfo()
    {
    }

    public AnswersInfo(string a, string b, string c, string d)
    {
        (A, B, C, D) = (a, b, c, d);
    }

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

    /*public IEnumerator<string> GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
        yield return D;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();*/
}