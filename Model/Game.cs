using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model
{
    public sealed class Game : ReactiveObject
    {
        #region Fields

        private IDisposable _showCurrentSumSubscription;
        private IDisposable _hideCurrentSumSubscription;

        #endregion Fields

        public Game()
        {
            Quizbase = new Quizbase(this);

            Lifelines = new Lifelines();
            Lifelines.Initialize(new Lifeline[]
            {
                new FiftyFifty(this),
                new PhoneAFriend(this),
                new AskTheAudience(this),
                new DoubleDip(this),
                //new AskTheHost(this)
            });

            Levels = new Levels();
            Levels.Initialize(new Level[]
            {
                new Level(1, 500),
                new Level(2, 1000),
                new Level(3, 2000),
                new Level(4, 3000),
                new Level(5, 5000),
                new Level(6, 10000),
                new Level(7, 15000),
                new Level(8, 25000),
                new Level(9, 50000),
                new Level(10, 100000),
                new Level(11, 200000),
                new Level(12, 400000),
                new Level(13, 800000),
                new Level(14, 1500000),
                new Level(15, 3000000)
            });

            Lozenge = new Lozenge(this);

            CurrentLevel = Levels[0];

            this.WhenAnyValue(game => game.CurrentLevel)
                .Subscribe(lvl => AudioManager.Instance.CurrentLevel = lvl);

            SmallMoneyTree = new LevelObject(this, ResourceManager.GetResourceGraphicsName("smt {0}", "png", "Money Trees", "Small"), lvl => lvl >= 0 && lvl <= 15);
            BigMoneyTree = new LevelObject(this, ResourceManager.GetResourceGraphicsName("bmt {0}", "png", "Money Trees", "Big"), lvl => lvl >= 0 && lvl <= 15);
            CurrentSum = new LevelObject(this, ResourceManager.GetResourceGraphicsName("sum {0}", "png", "Sums"), lvl => lvl >= 1 && lvl <= 14) { Level = 1 };
            Winnings = new LevelObject(this, ResourceManager.GetResourceGraphicsName("winnings {0}", "png", "Winnings"), lvl => lvl >= 0 && lvl <= 15);

            this.WhenAnyValue(game => game.CurrentLevel)
                .Select(lvl => (byte)(lvl.Number - 1))
                .Subscribe(lvl =>
                {
                    SmallMoneyTree.Level = lvl;
                    BigMoneyTree.Level = lvl;
                    CurrentSum.Level = lvl;
                });
        }

        #region Properties

        public Quizbase Quizbase { get; }
        [Reactive] public Quiz CurrentQuiz { get; set; }

        public Lozenge Lozenge { get; }

        public Levels Levels { get; }
        [Reactive] public Level CurrentLevel { get; set; }
        public bool IsFirstStage => CurrentLevel.Number <= 5;

        [Reactive] public bool HasWalkedAway { get; set; }

        public Lifelines Lifelines { get; }

        public LevelObject SmallMoneyTree { get; }
        public LevelObject BigMoneyTree { get; }
        public LevelObject CurrentSum { get; }
        public LevelObject Winnings { get; }

        #endregion Properties

        #region Methods

        public void Clear(bool firstStage = false)
        {
            CurrentSum.IsShown = false;
            Winnings.IsShown = false;
            BigMoneyTree.IsShown = false;

            if (!firstStage)
                SmallMoneyTree.IsShown = false;

            Lozenge.Clear();
        }

        public void LightsDown()
        {
            if (!IsFirstStage || CurrentLevel.Number == 1)
                AudioManager.Instance.PlayLightsDown();

            _showCurrentSumSubscription?.Dispose();
            _hideCurrentSumSubscription?.Dispose();

            Clear(IsFirstStage && CurrentLevel.Number != 1);

            Quizbase.SelectedQuiz = Quizbase.Collection.FirstOrDefault(q => q.Level == CurrentLevel.Number);
        }

        public void ShowQuestion()
        {
            if (!IsFirstStage || CurrentLevel.Number == 1)
                AudioManager.Instance.PlayBackground();

            BigMoneyTree.IsShown = false;
            SmallMoneyTree.IsShown = true;
            Lozenge.IsShown = true;
            Lozenge.LifelinesPanel.IsShown = false;
        }

        public void LockAnswer(AnswerID answerID)
        {
            if (!HasWalkedAway)
            {
                bool isDoubleDip = Lifelines.IsDoubleDipActivated(out DoubleDip doubleDip);

                if (isDoubleDip)
                    AudioManager.Instance.Play(doubleDip.FinalAnswerAudio);
                else if (!IsFirstStage)
                    AudioManager.Instance.PlayFinalAnswer();
            }

            Lozenge.LockAnswer(answerID);
        }

        public void RevealCorrect()
        {
            if (HasWalkedAway)
            {
                Lozenge.RevealCorrect();
            }
            else
            {
                bool isCorrect = Lozenge.Locked == CurrentQuiz.Correct;
                bool isDoubleDip = Lifelines.IsDoubleDipActivated(out DoubleDip doubleDip);

                if (isCorrect)
                {
                    if (isDoubleDip)
                    {
                        doubleDip.Mode = DoubleDipMode.Deactivated;
                        doubleDip.IsExecuting = false;
                    }

                    LaunchCurrentSumTimer(Lozenge.RevealCorrect());
                    AudioManager.Instance.PlayCorrect();
                }
                else
                {
                    if (isDoubleDip && doubleDip.Mode == DoubleDipMode.FirstAnswer)
                    {
                        doubleDip.Mode = DoubleDipMode.SecondAnswer;
                        Lozenge[Lozenge.Locked].IsShown = false;
                        Lozenge.Locked = AnswerID.None;

                        AudioManager.Instance.Play(Audio.DDWrong);
                    }
                    else
                    {
                        if (isDoubleDip)
                        {
                            doubleDip.Mode = DoubleDipMode.Deactivated;
                            doubleDip.IsExecuting = false;
                        }

                        Lozenge.RevealCorrect();
                        SmallMoneyTree.IsShown = false;

                        Winnings.Level = (byte)(CurrentLevel.Number > 10 ? 10 : (CurrentLevel.Number > 5 ? 5 : 0));

                        AudioManager.Instance.PlayWrong();
                    }
                }
            }
        }

        private void LaunchCurrentSumTimer(RevealCorrectType revealCorrectType)
        {
            if (CurrentLevel.Number < 15)
            {
                TimeSpan timeSpanShow = TimeSpan.FromMilliseconds(revealCorrectType == RevealCorrectType.Quick ? 1500 : 2000);
                TimeSpan timeSpanHide = TimeSpan.FromMilliseconds(revealCorrectType == RevealCorrectType.Quick ? 2000 : 5000);

                _showCurrentSumSubscription = Observable.Timer(timeSpanShow, RxApp.MainThreadScheduler).Subscribe(v =>
                {
                    CurrentLevel = Levels.Collection.FirstOrDefault(lvl => lvl.Number == CurrentLevel.Number + 1);
                    Clear(CurrentLevel.Number <= 5);
                    CurrentSum.IsShown = true;

                    _hideCurrentSumSubscription = Observable.Timer(timeSpanHide, RxApp.MainThreadScheduler).Subscribe(v => { CurrentSum.IsShown = false; });
                });
            }
            else
            {
                TimeSpan timeSpanShow = TimeSpan.FromMilliseconds(revealCorrectType == RevealCorrectType.Quick ? 2500 : 3000);

                _showCurrentSumSubscription = Observable.Timer(timeSpanShow, RxApp.MainThreadScheduler).Subscribe(v =>
                {
                    Winnings.Level = 15;
                    Winnings.IsShown = true;

                    Lozenge.IsShown = false;
                });

                SmallMoneyTree.IsShown = false;
                SmallMoneyTree.Level = 15;
                BigMoneyTree.Level = 15;
            }
        }

        public void Walkaway()
        {
            if (!HasWalkedAway)
            {
                HasWalkedAway = true;
                AudioManager.Instance.PlayWalkaway();

                SmallMoneyTree.IsShown = false;

                Winnings.Level = (byte)(CurrentLevel.Number - 1);
                Observable.Timer(TimeSpan.FromMilliseconds(3000), RxApp.MainThreadScheduler).Subscribe(v => { Winnings.IsShown = true; });
            }
        }

        #endregion Methods
    }
}