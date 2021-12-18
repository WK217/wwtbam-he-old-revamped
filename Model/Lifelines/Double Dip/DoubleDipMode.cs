using System.ComponentModel;

namespace WwtbamOld.Model;

public enum DoubleDipMode
{
    [Description("Не активирован")]
    Deactivated,

    [Description("Первый ответ")]
    FirstAnswer,

    [Description("Второй ответ")]
    SecondAnswer
}
