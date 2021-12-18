using System.ComponentModel;

namespace WwtbamOld.Media.Audio;

public enum Audio : byte
{
    [Description("Предварительный луп")]
    [AudioProperties("wwtbam loop.mp3", loop: true)]
    PreLoop,

    [Description("Заставка")]
    [AudioProperties("wwtbam opening.mp3")]
    Opening,

    [AudioProperties("wwtbam meeting.mp3")]
    [Description("Встреча игрока")]
    Meeting,

    [Description("Фоновая музыка")]
    [AudioProperties("wwtbam background.mp3", loop: true)]
    BackgroundGeneral,

    [Description("Правила игры")]
    [AudioProperties("wwtbam rules explanation.mp3", loop: true)]
    RulesExplanation,

    [AudioProperties("wwtbam rules lifeline 1.mp3", playerNumber: 2)]
    [Description("Пинг подсказки #1")]
    Ping1,

    [AudioProperties("wwtbam rules lifeline 2.mp3", playerNumber: 2)]
    [Description("Пинг подсказки #2")]
    Ping2,

    [AudioProperties("wwtbam rules lifeline 3.mp3", playerNumber: 2)]
    [Description("Пинг подсказки #3")]
    Ping3,

    [AudioProperties("wwtbam rules lifeline 4.mp3", playerNumber: 2)]
    [Description("Пинг подсказки #4")]
    Ping4,

    [AudioProperties("wwtbam rules end.mp3")]
    [Description("Окончание объяснения правил")]
    RulesEnd,

    [Description("Let's Play")]
    [AudioProperties("wwtbam lets play.mp3")]
    LetsPlay,

    [AudioProperties("wwtbam ld 6.mp3")]
    [Description("Lights Down (вопрос 6)")]
    LightsDown6,

    [Description("Lights Down (вопрос 7)")]
    [AudioProperties("wwtbam ld 7.mp3")]
    LightsDown7,

    [AudioProperties("wwtbam ld 8.mp3")]
    [Description("Lights Down (вопрос 8)")]
    LightsDown8,

    [AudioProperties("wwtbam ld 9.mp3")]
    [Description("Lights Down (вопрос 9)")]
    LightsDown9,

    [AudioProperties("wwtbam ld 10.mp3")]
    [Description("Lights Down (вопрос 10)")]
    LightsDown10,

    [AudioProperties("wwtbam ld 11.mp3")]
    [Description("Lights Down (вопрос 11)")]
    LightsDown11,

    [AudioProperties("wwtbam ld 12.mp3")]
    [Description("Lights Down (вопрос 12)")]
    LightsDown12,

    [AudioProperties("wwtbam ld 13.mp3")]
    [Description("Lights Down (вопрос 13)")]
    LightsDown13,

    [Description("Lights Down (вопрос 14)")]
    [AudioProperties("wwtbam ld 14.mp3")]
    LightsDown14,

    [AudioProperties("wwtbam ld 15.mp3")]
    [Description("Lights Down (вопрос 15)")]
    LightsDown15,

    [Description("Фон (вопросы 1-5)")]
    [AudioProperties("wwtbam bg 1.mp3", loop: true)]
    Background1,

    [AudioProperties("wwtbam bg 6.mp3", loop: true)]
    [Description("Фон (вопрос 6)")]
    Background6,

    [AudioProperties("wwtbam bg 7.mp3", loop: true)]
    [Description("Фон (вопрос 7)")]
    Background7,

    [AudioProperties("wwtbam bg 8.mp3", loop: true)]
    [Description("Фон (вопрос 8)")]
    Background8,

    [AudioProperties("wwtbam bg 9.mp3", loop: true)]
    [Description("Фон (вопрос 9)")]
    Background9,

    [AudioProperties("wwtbam bg 10.mp3", loop: true)]
    [Description("Фон (вопрос 10)")]
    Background10,

    [AudioProperties("wwtbam bg 11.mp3", loop: true)]
    [Description("Фон (вопрос 11)")]
    Background11,

    [Description("Фон (вопрос 12)")]
    [AudioProperties("wwtbam bg 12.mp3", loop: true)]
    Background12,

    [AudioProperties("wwtbam bg 13.mp3", loop: true)]
    [Description("Фон (вопрос 13)")]
    Background13,

    [AudioProperties("wwtbam bg 14.mp3", loop: true)]
    [Description("Фон (вопрос 14)")]
    Background14,

    [AudioProperties("wwtbam bg 15.mp3", loop: true)]
    [Description("Фон (вопрос 15)")]
    Background15,

    [AudioProperties("wwtbam final 6.mp3")]
    [Description("Принятие ответа (вопрос 6)")]
    FinalAnswer6,

    [AudioProperties("wwtbam final 7.mp3")]
    [Description("Принятие ответа (вопрос 7)")]
    FinalAnswer7,

    [AudioProperties("wwtbam final 8.mp3")]
    [Description("Принятие ответа (вопрос 8)")]
    FinalAnswer8,

    [AudioProperties("wwtbam final 9.mp3")]
    [Description("Принятие ответа (вопрос 9)")]
    FinalAnswer9,

    [Description("Принятие ответа (вопрос 10)")]
    [AudioProperties("wwtbam final 10.mp3")]
    FinalAnswer10,

    [AudioProperties("wwtbam final 11.mp3")]
    [Description("Принятие ответа (вопрос 11)")]
    FinalAnswer11,

    [AudioProperties("wwtbam final 12.mp3")]
    [Description("Принятие ответа (вопрос 12)")]
    FinalAnswer12,

    [AudioProperties("wwtbam final 13.mp3")]
    [Description("Принятие ответа (вопрос 13)")]
    FinalAnswer13,

    [AudioProperties("wwtbam final 14.mp3")]
    [Description("Принятие ответа (вопрос 14)")]
    FinalAnswer14,

    [AudioProperties("wwtbam final 15.mp3")]
    [Description("Принятие ответа (вопрос 15)")]
    FinalAnswer15,

    [AudioProperties("wwtbam correct 1.mp3", 2)]
    [Description("Верный ответ (вопросы 1-4)")]
    Correct1,

    [Description("Верный ответ (вопрос 5)")]
    [AudioProperties("wwtbam correct 5.mp3")]
    Correct5,

    [AudioProperties("wwtbam correct 6.mp3")]
    [Description("Верный ответ (вопрос 6)")]
    Correct6,

    [AudioProperties("wwtbam correct 7.mp3")]
    [Description("Верный ответ (вопрос 7)")]
    Correct7,

    [AudioProperties("wwtbam correct 8.mp3")]
    [Description("Верный ответ (вопрос 8)")]
    Correct8,

    [AudioProperties("wwtbam correct 9.mp3")]
    [Description("Верный ответ (вопрос 9)")]
    Correct9,

    [AudioProperties("wwtbam correct 10.mp3")]
    [Description("Верный ответ (вопрос 10)")]
    Correct10,

    [Description("Верный ответ (вопрос 11)")]
    [AudioProperties("wwtbam correct 11.mp3")]
    Correct11,

    [Description("Верный ответ (вопрос 12)")]
    [AudioProperties("wwtbam correct 12.mp3")]
    Correct12,

    [AudioProperties("wwtbam correct 13.mp3")]
    [Description("Верный ответ (вопрос 13)")]
    Correct13,

    [AudioProperties("wwtbam correct 14.mp3")]
    [Description("Верный ответ (вопрос 14)")]
    Correct14,

    [Description("Верный ответ (вопрос 15)")]
    [AudioProperties("wwtbam correct 15.mp3")]
    Correct15,

    [Description("Неверный ответ (вопросы 1-5)")]
    [AudioProperties("wwtbam wrong 1.mp3")]
    Wrong1,

    [AudioProperties("wwtbam wrong 6.mp3")]
    [Description("Неверный ответ (вопрос 6)")]
    Wrong6,

    [AudioProperties("wwtbam wrong 7.mp3")]
    [Description("Неверный ответ (вопрос 7)")]
    Wrong7,

    [AudioProperties("wwtbam wrong 8.mp3")]
    [Description("Неверный ответ (вопрос 8)")]
    Wrong8,

    [AudioProperties("wwtbam wrong 9.mp3")]
    [Description("Неверный ответ (вопрос 9)")]
    Wrong9,

    [AudioProperties("wwtbam wrong 10.mp3")]
    [Description("Неверный ответ (вопрос 10)")]
    Wrong10,

    [AudioProperties("wwtbam wrong 11.mp3")]
    [Description("Неверный ответ (вопрос 11)")]
    Wrong11,

    [AudioProperties("wwtbam wrong 12.mp3")]
    [Description("Неверный ответ (вопрос 12)")]
    Wrong12,

    [Description("Неверный ответ (вопрос 13)")]
    [AudioProperties("wwtbam wrong 13.mp3")]
    Wrong13,

    [AudioProperties("wwtbam wrong 14.mp3")]
    [Description("Неверный ответ (вопрос 14)")]
    Wrong14,

    [AudioProperties("wwtbam wrong 15.mp3")]
    [Description("Неверный ответ (вопрос 15)")]
    Wrong15,

    [AudioProperties("wwtbam fifty.mp3", 2)]
    [Description("«50 на 50»: использование")]
    FiftyUse,

    [AudioProperties("wwtbam phone pre.mp3", loop: true)]
    [Description("«Помощь друга»: активация")]
    PhoneUse,

    [AudioProperties("wwtbam phone timer.mp3")]
    [Description("«Помощь друга»: начало отсчёта")]
    PhoneStart,

    [AudioProperties("wwtbam phone stop.mp3")]
    [Description("«Помощь друга»: остановка отсчёта")]
    PhoneStop,

    [Description("«Три мудреца»: активация")]
    [AudioProperties("wwtbam twm pre.mp3", loop: true)]
    TWMUse,

    [AudioProperties("wwtbam twm timer loop.mp3", loop: true)]
    [Description("«Три мудреца»: фон отсчёта")]
    TWMCountdown,

    [AudioProperties("wwtbam twm timer start.mp3", playerNumber: 2)]
    [Description("«Три мудреца»: начало отсчёта")]
    TWMStart,

    [AudioProperties("wwtbam twm stop.mp3")]
    [Description("«Три мудреца»: остановка отсчёта")]
    TWMStop,

    [AudioProperties("wwtbam ath loop.mp3", loop: true)]
    [Description("«Помощь ведущего»: начало")]
    AtHStart,

    [AudioProperties("wwtbam ath execute.mp3", playerNumber: 2)]
    [Description("«Помощь ведущего»: применение")]
    AtHExecute,

    [AudioProperties("wwtbam ath stop.mp3")]
    [Description("«Помощь ведущего»: конец")]
    AtHStop,

    [AudioProperties("wwtbam ata pre.mp3", loop: true)]
    [Description("«Помощь зала»: активация»")]
    AtAUse,

    [AudioProperties("wwtbam ata voting.mp3")]
    [Description("«Помощь зала»: начало голосования")]
    AtAStart,

    [AudioProperties("wwtbam ata stop.mp3")]
    [Description("«Помощь зала»: остановка голосования")]
    AtAStop,

    [AudioProperties("wwtbam ata show.mp3")]
    [Description("«Помощь зала»: показ результатов")]
    AtAShow,

    [Description("«Двойной ответ»: активация")]
    [AudioProperties("wwtbam dd activate.mp3")]
    DDUse,

    [AudioProperties("wwtbam dd activate bg.mp3", loop: true)]
    [Description("«Двойной ответ»: активация (фон)")]
    DDUseLoop,

    [Description("«Двойной ответ»: принятие первого ответа")]
    [AudioProperties("wwtbam dd 1.mp3")]
    DDLock1,

    [Description("«Двойной ответ»: неправильный ответ")]
    [AudioProperties("wwtbam dd wrong.mp3")]
    DDWrong,

    [AudioProperties("wwtbam dd wrong bg.mp3", loop: true)]
    [Description("«Двойной ответ»: неправильный ответ (фон)")]
    DDWrongLoop,

    [AudioProperties("wwtbam dd 2.mp3")]
    [Description("«Двойной ответ»: принятие второго ответа")]
    DDLock2,

    [AudioProperties("wwtbam small walkaway.mp3")]
    [Description("Взятие денег: малая сумма")]
    SmallWalkaway,

    [AudioProperties("wwtbam small walkaway 2.mp3")]
    [Description("Взятие денег: малая сумма (2)")]
    SmallWalkaway2,

    [Description("Взятие денег: большая сумма")]
    [AudioProperties("wwtbam big walkaway.mp3")]
    BigWalkaway,

    [AudioProperties("wwtbam big walkaway 2.mp3")]
    [Description("Взятие денег: большая сумма (2)")]
    BigWalkaway2,

    [AudioProperties("wwtbam commercial in.mp3")]
    [Description("Уход на перерыв")]
    CommercialIn,

    [AudioProperties("wwtbam commercial out.mp3")]
    [Description("Возврат с перерыва")]
    CommercialOut,

    [Description("Уход игрока")]
    [AudioProperties("wwtbam goodbye.mp3")]
    Goodbye,

    [AudioProperties("wwtbam siren.mp3", 2)]
    [Description("Финальная сирена")]
    FinalSiren,

    [AudioProperties("wwtbam closing.mp3")]
    [Description("Финальные титры")]
    Closing,

    [AudioProperties("wwtbam silence.mp3")]
    [Description("Нет звука (канал 1)")]
    Silence1,

    [AudioProperties("wwtbam silence.mp3", playerNumber: 2)]
    [Description("Нет звука (канал 2)")]
    Silence2
}
