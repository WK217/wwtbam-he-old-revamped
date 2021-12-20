using System.Xml.Serialization;

namespace WwtbamOld.Model;

public enum AnswerID : byte
{
    [XmlEnum(Name = "-")]
    None,

    [XmlEnum(Name = "a")]
    A,

    [XmlEnum(Name = "b")]
    B,

    [XmlEnum(Name = "c")]
    C,

    [XmlEnum(Name = "d")]
    D
}