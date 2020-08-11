using System.Xml;

namespace WwtbamOld.Model
{
    public sealed class Question : MediaTextContent
    {
        public Question(XmlNode xmlNode) : base(xmlNode)
        {
        }

        public Question(string text) : base(text)
        {
        }
    }
}