using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

[Description("«Помощь зала»")]
public sealed class AskTheAudience : Lifeline
{
    #region Basic Properties

    public override byte ID => 3;
    public override string Code => "ata";
    public override string Name => "«Помощь зала»";

    #endregion Basic Properties

    #region Fields

    private readonly ObservableAsPropertyHelper<uint> _votesSum;

    private IObservable<long> _voteTimerObservable;
    private IDisposable _voteTimerSubscription;

    #endregion Fields

    public AskTheAudience(Game game) : base(game)
    {
        _votesSum = this.WhenAnyValue(ata => ata.A.Votes,
                                      ata => ata.B.Votes,
                                      ata => ata.C.Votes,
                                      ata => ata.D.Votes,
                                      (a, b, c, d) => a + b + c + d)
                        .ToProperty(this, nameof(VotesSum));

        A = new AskTheAudienceAnswer(this) { Model = game.Lozenge.A };
        B = new AskTheAudienceAnswer(this) { Model = game.Lozenge.B };
        C = new AskTheAudienceAnswer(this) { Model = game.Lozenge.C };
        D = new AskTheAudienceAnswer(this) { Model = game.Lozenge.D };
    }

    #region Properties

    public AskTheAudienceAnswer A { get; }
    public AskTheAudienceAnswer B { get; }
    public AskTheAudienceAnswer C { get; }
    public AskTheAudienceAnswer D { get; }

    public IEnumerable<AskTheAudienceAnswer> Answers
    {
        get
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    [Reactive] public AskTheAudienceDataType DataType { get; set; }

    public uint VotesSum => _votesSum.Value;

    [Reactive] public bool IsTableShown { get; set; }
    [Reactive] public bool AreResultsShown { get; set; }

    #endregion Properties

    #region Methods

    private void Execute(bool execute = true)
    {
        IsExecuting = execute;
        IsTableShown = execute;
        AreResultsShown = false;
        State = execute ? LifelineState.Activated : LifelineState.Disabled;

        if (!execute)
            _voteTimerSubscription?.Dispose();
    }

    public void Initiate()
    {
        Execute();
        AudioManager.Play(Audio.AtAUse);
    }

    public override void Activate()
    {
        Execute();
        AudioManager.Play(Audio.AtAStart);

        _voteTimerObservable = Observable.Timer(TimeSpan.FromSeconds(60), RxApp.MainThreadScheduler);
        _voteTimerSubscription?.Dispose();
        _voteTimerSubscription = _voteTimerObservable.Do(x => Deactivate(ahead: false))
                                                     .Subscribe();
    }

    public void Deactivate(bool ahead = true)
    {
        Execute();
        AreResultsShown = true;

        if (ahead)
        {
            _voteTimerSubscription?.Dispose();
            AudioManager.Play(Audio.AtAShow);
        }
    }

    public void End()
    {
        Execute(false);
        AudioManager.Instance.PlayBackground();
    }

    #endregion Methods
}