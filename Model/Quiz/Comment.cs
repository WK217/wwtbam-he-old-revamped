using System.Xml;

namespace WwtbamOld.Model
{
    public sealed class Comment : MediaTextContent
    {
        public Comment(XmlNode xmlNode) : base(xmlNode)
        {
        }

        public Comment(string text) : base(text)
        {
        }
    }
}