using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Xml;

namespace WwtbamOld.Model
{
    public abstract class MediaTextContent : ReactiveObject
    {
        public MediaTextContent(XmlNode xmlNode)
        {
            Text = xmlNode.SelectSingleNode("text").InnerText;
            Photo = xmlNode.SelectSingleNode("photo")?.InnerText ?? string.Empty;
        }

        public MediaTextContent(string text)
        {
            Text = text;
        }

        [Reactive] public string Text { get; set; }
        [Reactive] public string Photo { get; set; }
        [Reactive] public bool IsShown { get; set; }

        public override string ToString() => Text;
    }
}