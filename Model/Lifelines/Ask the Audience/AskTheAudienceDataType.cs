using System.ComponentModel;

namespace WwtbamOld.Model
{
    public enum AskTheAudienceDataType : byte
    {
        [Description("Значение")]
        Normal,

        [Description("Проценты")]
        Percentage
    }
}