using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

[Description("«Помощь ведущего»")]
public sealed class AskTheHost : Lifeline
{
    #region Basic Properties

    public override byte ID => 6;
    public override string Code => "ath";
    public override string Name => "«Помощь ведущего»";

    #endregion Basic Properties

    #region Fields

    private static readonly TimeSpan _hideDelay = TimeSpan.FromMilliseconds(4000);

    private IDisposable _hideSubscription;

    #endregion Fields

    public AskTheHost(Game game)
        : base(game, true)
    {
    }

    #region Properties

    [Reactive] public AnswerID PlayerAnswer1 { get; set; }
    [Reactive] public AnswerID PlayerAnswer2 { get; set; }
    [Reactive] public AnswerID HostAnswer { get; set; }

    public IObservable<bool> CanVerifyPlayerAnswers => this.WhenAnyValue(ath => ath.PlayerAnswer1,
                                                                         ath => ath.PlayerAnswer2,
                                                                         (ans1, ans2) => ans1 != AnswerID.None && ans2 != AnswerID.None && ans1 != ans2);

    public IObservable<bool> CanVerifyHostAnswer => this.WhenAnyValue(ath => ath._game.CurrentQuiz.Correct,
                                                                ath => ath.HostAnswer,
                                                                (correct, ans) => ans != AnswerID.None && correct != ans);

    #endregion Properties

    #region Methods

    private void Execute(bool execute = true)
    {
        IsExecuting = execute;
        State = execute ? LifelineState.Activated : LifelineState.Disabled;
    }

    public override void Activate()
    {
        Execute();
        AudioManager.Play(Audio.AtHStart);
    }

    public void Deactivate()
    {
        Execute(false);
        AudioManager.Play(Audio.AtHStop);

        _hideSubscription?.Dispose();
        _hideSubscription = Observable.Timer(_hideDelay, RxApp.MainThreadScheduler)
                                      .Subscribe(_ => AudioManager.Instance.PlayBackground());
    }

    public void VerifyAnswers(bool player = true)
    {
        bool correct = player ? FindCorrect(_game.CurrentQuiz.Correct, PlayerAnswer1, PlayerAnswer2) : FindCorrect(_game.CurrentQuiz.Correct, HostAnswer);

        if (player)
        {
            for (AnswerID id = 0; id <= AnswerID.D; id++)
                _game.Lozenge[id].IsShown = correct ? (id == PlayerAnswer1 || id == PlayerAnswer2) : (id != PlayerAnswer1 && id != PlayerAnswer2);

            PlayerAnswer1 = AnswerID.None;
            PlayerAnswer2 = AnswerID.None;
        }
        else if (!correct)
        {
            _game.Lozenge[HostAnswer].IsShown = false;
            HostAnswer = AnswerID.None;
        }

        if (player || !correct)
            AudioManager.Play(Audio.AtHExecute);
    }

    private static bool FindCorrect(AnswerID correct, params AnswerID[] answers)
    {
        foreach (AnswerID answer in answers)
            if (answer == correct)
                return true;

        return false;
    }

    #endregion Methods
}
