using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

[Description("«50 на 50»")]
public sealed class FiftyFifty : Lifeline
{
    #region Basic Properties

    public override byte ID => 1;
    public override string Code => "fifty";
    public override string Name => "«50 на 50»";

    #endregion Basic Properties

    #region Fields

    private readonly ObservableAsPropertyHelper<AnswerID> _correctID;

    private IObservable<long> _pingTimerObservable;
    private IDisposable _pingTimerSubscription;

    #endregion Fields

    public FiftyFifty(Game game)
        : base(game)
    {
        _correctID = this.WhenAnyValue(fifty => fifty._game.Lozenge.Correct)
                         .ToProperty(this, nameof(CorrectID));

        this.WhenAnyValue(fifty => fifty._game.Lozenge.Alternative)
            .BindTo(this, fifty => fifty.Alternative);
    }

    #region Properties

    public AnswerID CorrectID => _correctID is not null ? _correctID.Value : AnswerID.None;
    [Reactive] public AnswerID Alternative { get; set; }

    public override IObservable<bool> CanActivate => this.WhenAnyValue(fifty => fifty.IsEnabled, fifty => fifty.Alternative,
        (enabled, alternative) => enabled && alternative != AnswerID.None && !_game.Lozenge[alternative].IsCorrect);

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
        AudioManager.Play(Audio.FiftyUse);

        for (AnswerID id = 0; id <= AnswerID.D; id++)
            _game.Lozenge[id].IsShown = id == _game.Lozenge.Correct || id == Alternative;

        Alternative = AnswerID.None;

        _pingTimerObservable = Observable.Timer(TimeSpan.FromMilliseconds(150), RxApp.MainThreadScheduler);
        _pingTimerSubscription = _pingTimerObservable.Subscribe(x => { Execute(false); });
    }

    #endregion Methods
}