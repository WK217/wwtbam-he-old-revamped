using System.ComponentModel;

namespace WwtbamOld.Media.Audio
{
    public enum Audio : byte
    {
        [Description("Предварительный луп")]
        [AudioProperties("loop", true)]
        PreLoop,

        [Description("Заставка")]
        [AudioProperties("opening")]
        Opening,

        [AudioProperties("meeting")]
        [Description("Встреча игрока")]
        Meeting,

        [Description("Фоновая музыка")]
        [AudioProperties("background", true)]
        BackgroundGeneral,

        [Description("Правила игры")]
        [AudioProperties("rules explanation", true)]
        RulesExplanation,

        [AudioProperties("rules lifeline 1", 2)]
        [Description("Демо «50 на 50»")]
        PingFifty,

        [AudioProperties("rules lifeline 2", 2)]
        [Description("Демо «Помощь друга»")]
        PingPhone,

        [AudioProperties("rules lifeline 3", 2)]
        [Description("Демо «Помощь зала»")]
        PingAtA,

        [AudioProperties("rules lifeline 4", 2)]
        [Description("Демо «Двойной ответ»")]
        PingDoubleDip,

        [AudioProperties("rules end")]
        [Description("Окончание объяснения правил")]
        RulesEnd,

        [Description("Let's Play")]
        [AudioProperties("lets play")]
        LetsPlay,

        [AudioProperties("ld 6")]
        [Description("Lights Down (вопрос 6)")]
        LightsDown6,

        [Description("Lights Down (вопрос 7)")]
        [AudioProperties("ld 7")]
        LightsDown7,

        [AudioProperties("ld 8")]
        [Description("Lights Down (вопрос 8)")]
        LightsDown8,

        [AudioProperties("ld 9")]
        [Description("Lights Down (вопрос 9)")]
        LightsDown9,

        [AudioProperties("ld 10")]
        [Description("Lights Down (вопрос 10)")]
        LightsDown10,

        [AudioProperties("ld 11")]
        [Description("Lights Down (вопрос 11)")]
        LightsDown11,

        [AudioProperties("ld 12")]
        [Description("Lights Down (вопрос 12)")]
        LightsDown12,

        [AudioProperties("ld 13")]
        [Description("Lights Down (вопрос 13)")]
        LightsDown13,

        [Description("Lights Down (вопрос 14)")]
        [AudioProperties("ld 14")]
        LightsDown14,

        [AudioProperties("ld 15")]
        [Description("Lights Down (вопрос 15)")]
        LightsDown15,

        [Description("Фон (вопросы 1-5)")]
        [AudioProperties("bg 1", true)]
        Background1,

        [AudioProperties("bg 6", true)]
        [Description("Фон (вопрос 6)")]
        Background6,

        [AudioProperties("bg 7", true)]
        [Description("Фон (вопрос 7)")]
        Background7,

        [AudioProperties("bg 8", true)]
        [Description("Фон (вопрос 8)")]
        Background8,

        [AudioProperties("bg 9", true)]
        [Description("Фон (вопрос 9)")]
        Background9,

        [AudioProperties("bg 10", true)]
        [Description("Фон (вопрос 10)")]
        Background10,

        [AudioProperties("bg 11", true)]
        [Description("Фон (вопрос 11)")]
        Background11,

        [Description("Фон (вопрос 12)")]
        [AudioProperties("bg 12", true)]
        Background12,

        [AudioProperties("bg 13", true)]
        [Description("Фон (вопрос 13)")]
        Background13,

        [AudioProperties("bg 14", true)]
        [Description("Фон (вопрос 14)")]
        Background14,

        [AudioProperties("bg 15", true)]
        [Description("Фон (вопрос 15)")]
        Background15,

        [AudioProperties("final 6")]
        [Description("Принятие ответа (вопрос 6)")]
        FinalAnswer6,

        [AudioProperties("final 7")]
        [Description("Принятие ответа (вопрос 7)")]
        FinalAnswer7,

        [AudioProperties("final 8")]
        [Description("Принятие ответа (вопрос 8)")]
        FinalAnswer8,

        [AudioProperties("final 9")]
        [Description("Принятие ответа (вопрос 9)")]
        FinalAnswer9,

        [Description("Принятие ответа (вопрос 10)")]
        [AudioProperties("final 10")]
        FinalAnswer10,

        [AudioProperties("final 11")]
        [Description("Принятие ответа (вопрос 11)")]
        FinalAnswer11,

        [AudioProperties("final 12")]
        [Description("Принятие ответа (вопрос 12)")]
        FinalAnswer12,

        [AudioProperties("final 13")]
        [Description("Принятие ответа (вопрос 13)")]
        FinalAnswer13,

        [AudioProperties("final 14")]
        [Description("Принятие ответа (вопрос 14)")]
        FinalAnswer14,

        [AudioProperties("final 15")]
        [Description("Принятие ответа (вопрос 15)")]
        FinalAnswer15,

        [AudioProperties("correct 1", 2)]
        [Description("Верный ответ (вопросы 1-4)")]
        Correct1,

        [Description("Верный ответ (вопрос 5)")]
        [AudioProperties("correct 5")]
        Correct5,

        [AudioProperties("correct 6")]
        [Description("Верный ответ (вопрос 6)")]
        Correct6,

        [AudioProperties("correct 7")]
        [Description("Верный ответ (вопрос 7)")]
        Correct7,

        [AudioProperties("correct 8")]
        [Description("Верный ответ (вопрос 8)")]
        Correct8,

        [AudioProperties("correct 9")]
        [Description("Верный ответ (вопрос 9)")]
        Correct9,

        [AudioProperties("correct 10")]
        [Description("Верный ответ (вопрос 10)")]
        Correct10,

        [Description("Верный ответ (вопрос 11)")]
        [AudioProperties("correct 11")]
        Correct11,

        [Description("Верный ответ (вопрос 12)")]
        [AudioProperties("correct 12")]
        Correct12,

        [AudioProperties("correct 13")]
        [Description("Верный ответ (вопрос 13)")]
        Correct13,

        [AudioProperties("correct 14")]
        [Description("Верный ответ (вопрос 14)")]
        Correct14,

        [Description("Верный ответ (вопрос 15)")]
        [AudioProperties("correct 15")]
        Correct15,

        [Description("Неверный ответ (вопросы 1-5)")]
        [AudioProperties("wrong 1")]
        Wrong1,

        [AudioProperties("wrong 6")]
        [Description("Неверный ответ (вопрос 6)")]
        Wrong6,

        [AudioProperties("wrong 7")]
        [Description("Неверный ответ (вопрос 7)")]
        Wrong7,

        [AudioProperties("wrong 8")]
        [Description("Неверный ответ (вопрос 8)")]
        Wrong8,

        [AudioProperties("wrong 9")]
        [Description("Неверный ответ (вопрос 9)")]
        Wrong9,

        [AudioProperties("wrong 10")]
        [Description("Неверный ответ (вопрос 10)")]
        Wrong10,

        [AudioProperties("wrong 11")]
        [Description("Неверный ответ (вопрос 11)")]
        Wrong11,

        [AudioProperties("wrong 12")]
        [Description("Неверный ответ (вопрос 12)")]
        Wrong12,

        [Description("Неверный ответ (вопрос 13)")]
        [AudioProperties("wrong 13")]
        Wrong13,

        [AudioProperties("wrong 14")]
        [Description("Неверный ответ (вопрос 14)")]
        Wrong14,

        [AudioProperties("wrong 15")]
        [Description("Неверный ответ (вопрос 15)")]
        Wrong15,

        [AudioProperties("fifty", 2)]
        [Description("«50 на 50»: использование")]
        FiftyUse,

        [AudioProperties("phone pre", true)]
        [Description("«Помощь друга»: активация")]
        PhoneUse,

        [AudioProperties("phone timer")]
        [Description("«Помощь друга»: начало отсчёта")]
        PhoneStart,

        [AudioProperties("phone stop")]
        [Description("«Помощь друга»: остановка отсчёта")]
        PhoneStop,

        [Description("«Три мудреца»: активация")]
        [AudioProperties("twm pre", true)]
        TWMUse,

        [AudioProperties("twm timer loop", true)]
        [Description("«Три мудреца»: начало отсчёта")]
        TWMStart,

        [AudioProperties("twm stop")]
        [Description("«Три мудреца»: остановка отсчёта")]
        TWMStop,

        [AudioProperties("ath loop", true)]
        [Description("«Помощь ведущего»: начало")]
        AtHStart,

        [AudioProperties("ath stop")]
        [Description("«Помощь ведущего»: конец")]
        AtHStop,

        [AudioProperties("ata pre", true)]
        [Description("«Помощь зала»: активация»")]
        AtAUse,

        [AudioProperties("ata voting")]
        [Description("«Помощь зала»: начало голосования")]
        AtAStart,

        [AudioProperties("ata stop")]
        [Description("«Помощь зала»: остановка голосования")]
        AtAStop,

        [AudioProperties("ata show")]
        [Description("«Помощь зала»: показ результатов")]
        AtAShow,

        [Description("«Двойной ответ»: активация")]
        [AudioProperties("dd activate")]
        DDUse,

        [AudioProperties("dd activate bg", true)]
        [Description("«Двойной ответ»: активация (фон)")]
        DDUseLoop,

        [Description("«Двойной ответ»: принятие первого ответа")]
        [AudioProperties("dd 1")]
        DDLock1,

        [Description("«Двойной ответ»: неправильный ответ")]
        [AudioProperties("dd wrong")]
        DDWrong,

        [AudioProperties("dd wrong bg", true)]
        [Description("«Двойной ответ»: неправильный ответ (фон)")]
        DDWrongLoop,

        [AudioProperties("dd 2")]
        [Description("«Двойной ответ»: принятие второго ответа")]
        DDLock2,

        [AudioProperties("small walkaway")]
        [Description("Взятие денег: малая сумма")]
        SmallWalkaway,

        [AudioProperties("small walkaway 2")]
        [Description("Взятие денег: малая сумма (2)")]
        SmallWalkaway2,

        [Description("Взятие денег: большая сумма")]
        [AudioProperties("big walkaway")]
        BigWalkaway,

        [AudioProperties("big walkaway 2")]
        [Description("Взятие денег: большая сумма (2)")]
        BigWalkaway2,

        [AudioProperties("commercial in")]
        [Description("Уход на перерыв")]
        CommercialIn,

        [AudioProperties("commercial out")]
        [Description("Возврат с перерыва")]
        CommercialOut,

        [Description("Уход игрока")]
        [AudioProperties("goodbye")]
        Goodbye,

        [AudioProperties("siren", 2)]
        [Description("Финальная сирена")]
        FinalSiren,

        [AudioProperties("closing")]
        [Description("Финальные титры")]
        Closing,

        [AudioProperties("silence")]
        [Description("Нет звука (канал 1)")]
        Silence1,

        [AudioProperties("silence", 2)]
        [Description("Нет звука (канал 2)")]
        Silence2
    }
}