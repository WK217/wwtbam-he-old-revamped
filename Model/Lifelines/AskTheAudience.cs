using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    [Description("«Помощь зала»")]
    public sealed class AskTheAudience : Lifeline
    {
        #region Basic Properties

        public override byte ID => 3;
        public override string Code => "ata";
        public override string Name => "«Помощь зала»";
        public override Audio PingSound => Audio.PingAtA;

        #endregion Basic Properties

        #region Fields

        /*private readonly ObservableCollectionExtended<AskTheAudienceAnswer> _collection;
        private readonly ReadOnlyObservableCollection<AskTheAudienceAnswer> _readOnlyCollection;*/

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

            A = new AskTheAudienceAnswer(this);
            B = new AskTheAudienceAnswer(this);
            C = new AskTheAudienceAnswer(this);
            D = new AskTheAudienceAnswer(this);

            this.WhenAnyValue(ata => ata._game.CurrentQuiz)
                .Where(q => q != null)
                .Subscribe(q =>
                {
                    A.Model = q.A;
                    B.Model = q.B;
                    C.Model = q.C;
                    D.Model = q.D;
                });
        }

        #region Properties

        [Reactive] public AskTheAudienceAnswer A { get; private set; }
        [Reactive] public AskTheAudienceAnswer B { get; private set; }
        [Reactive] public AskTheAudienceAnswer C { get; private set; }
        [Reactive] public AskTheAudienceAnswer D { get; private set; }

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
            _voteTimerSubscription = _voteTimerObservable.Subscribe(x => Deactivate(ahead: false));
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
}