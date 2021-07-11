using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Timers;
using WwtbamOld.Media.Audio;
using WwtbamOld.ViewModel;

namespace WwtbamOld.Model.Old
{
    public sealed class AskTheAudienceOld : LifelineOld
    {
        public override byte ID => 3;
        public override string Code => "ata";
        public override string Name => "«Помощь зала»";
        public override Audio PingSound => Audio.PingAtA;

        public AskTheAudienceOld(HostViewModel hostViewModel)
        {
            _hostViewModel = hostViewModel;

            Answers = new ObservableCollection<AskTheAudienceAnswerOld>();
            foreach (Answer answer in _hostViewModel.CurrentQuiz)
                Answers.Add(new AskTheAudienceAnswerOld(answer, this));

            InitiateCommand = ReactiveCommand.Create(() =>
            {
                Shown = true;
                ResultsShown = false;
                AudioManager.Play(Audio.AtAUse);
                State = LifelineState.Activated;
            });

            _activateCommand = ReactiveCommand.Create(() =>
            {
                _voteTimer = new Timer(60000.0);
                _voteTimer.Elapsed += (s, e) =>
                {
                    CloseVoting();
                    _voteTimer.Stop();
                };

                _voteTimer.Start();
                OpenVoting();
            });

            DeactivateCommand = ReactiveCommand.Create(() => { CloseVoting(); }, this.WhenAnyValue(ata => ata.VotingOpened));

            EndCommand = ReactiveCommand.Create(() =>
            {
                VotingOpened = false;
                Shown = false;
                ResultsShown = false;
                Enabled = false;
                AudioManager.Instance.PlayBackground();
            });
        }

        public ObservableCollection<AskTheAudienceAnswerOld> Answers { get; }

        private void OpenVoting()
        {
            Shown = true;
            ResultsShown = false;
            State = LifelineState.Activated;
            AudioManager.Play(Audio.AtAStart);
            VotingOpened = true;
        }

        private void CloseVoting()
        {
            Shown = true;
            ResultsShown = true;
            AudioManager.Play(Audio.AtAShow);
            VotingOpened = false;
        }

        public int VotesSum
        {
            get
            {
                int num = 0;
                foreach (AskTheAudienceAnswerOld askTheAudienceAnswer in Answers)
                    num += askTheAudienceAnswer.Votes;

                return num;
            }
        }

        public void UpdateVotes()
        {
            this.RaisePropertyChanged(nameof(VotesSum));
            foreach (AskTheAudienceAnswerOld askTheAudienceAnswer in Answers)
            {
                askTheAudienceAnswer.RaisePropertyChanged(nameof(AskTheAudienceAnswerOld.Votes));
                askTheAudienceAnswer.RaisePropertyChanged(nameof(AskTheAudienceAnswerOld.Percentage));
            }
        }

        public bool VotingOpened
        {
            get => _votingOpened;
            set
            {
                var oldValue = _votingOpened;
                this.RaiseAndSetIfChanged(ref _votingOpened, value);

                if (_votingOpened != oldValue && !value)
                    _voteTimer.Stop();
            }
        }

        public AskTheAudienceDataType DataType
        {
            get => _dataType;
            set
            {
                var oldValue = _dataType;
                this.RaiseAndSetIfChanged(ref _dataType, value);

                if (_dataType != oldValue)
                    foreach (AskTheAudienceAnswerOld askTheAudienceAnswer in Answers)
                        askTheAudienceAnswer.RaisePropertyChanged(nameof(AskTheAudienceAnswerOld.DataType));
            }
        }

        public bool Shown { get; set; }
        public bool ResultsShown { get; set; }

        public override ReactiveCommand<Unit, Unit> ActivateCommand => _activateCommand;
        public ReactiveCommand<Unit, Unit> InitiateCommand { get; }
        public ReactiveCommand<Unit, Unit> DeactivateCommand { get; }
        public ReactiveCommand<Unit, Unit> EndCommand { get; }

        private readonly HostViewModel _hostViewModel;
        private Timer _voteTimer;
        private bool _votingOpened;
        private AskTheAudienceDataType _dataType;
        private readonly ReactiveCommand<Unit, Unit> _activateCommand;
    }
}