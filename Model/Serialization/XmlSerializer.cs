using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WwtbamOld.Model;

public sealed class XmlSerializer : Serializer
{
    #region Fields

    public static readonly XmlSerializer Instance;
    private static readonly System.Xml.Serialization.XmlSerializer _quizzesSerializer;

    #endregion Fields

    static XmlSerializer()
    {
        Instance = new();
        _quizzesSerializer = new(type: typeof(List<Quiz>), root: new XmlRootAttribute("quizzes"));
    }

    private XmlSerializer()
    {
    }

    #region Properties

    public override string Extension => "xml";

    #endregion Properties

    #region Methods

    public override T Deserialize<T>(string xml)
    {
        using StringReader reader = new(xml);

        try
        {
            System.Xml.Serialization.XmlSerializer serializer;

            Type type = typeof(T);
            if (type.Equals(typeof(IEnumerable<Quiz>)))
                serializer = _quizzesSerializer;
            else
                serializer = new System.Xml.Serialization.XmlSerializer(type);

            return (T)serializer.Deserialize(reader);
        }
        catch (Exception)
        {
            return default;
        }
    }

    #endregion Methods
}