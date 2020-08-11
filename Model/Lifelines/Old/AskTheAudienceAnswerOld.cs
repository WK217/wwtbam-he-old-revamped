using ReactiveUI;

namespace WwtbamOld.Model.Old
{
    public sealed class AskTheAudienceAnswerOld : ReactiveObject
    {
        public AskTheAudienceAnswerOld(Answer answer, AskTheAudienceOld lifeline)
        {
            _answer = answer;
            _lifeline = lifeline;
        }

        public AnswerID ID => _answer.ID;

        public int Votes
        {
            get => _votes;
            set
            {
                if (_votes != value)
                {
                    _votes = value;
                    _lifeline.UpdateVotes();
                    this.RaisePropertyChanged(nameof(Votes));
                }
            }
        }

        public double Percentage => _lifeline.VotesSum != 0 ? Votes / (double)_lifeline.VotesSum * 100.0 : 0.0;

        public AskTheAudienceDataType DataType => _lifeline.DataType;

        private readonly Answer _answer;
        private readonly AskTheAudienceOld _lifeline;
        private int _votes;
    }
}