using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Windows.Media;

namespace WwtbamOld.Model;

public sealed class AskTheAudienceAnswer : ReactiveObject
{
    #region Fields

    private readonly AskTheAudience _lifeline;
    private readonly ObservableAsPropertyHelper<double> _share;
    private readonly ObservableAsPropertyHelper<string> _output;

    #endregion Fields

    public AskTheAudienceAnswer(AskTheAudience lifeline)
    {
        _lifeline = lifeline;

        _share = this.WhenAnyValue(answer => answer._lifeline.VotesSum, sum => sum > 0 ? (double)Votes / sum : 0)
                     .ToProperty(this, nameof(Share));

        this.WhenAnyValue(answer => answer.Model)
            .Do(_ => this.RaisePropertyChanged(nameof(ID)))
            .Subscribe();

        _output = this.WhenAnyValue(answer => answer._lifeline.DataType, answer => answer.Votes, answer => answer.Share,
                        (type, votes, share) => type == AskTheAudienceDataType.Normal ? string.Format("{0}", Votes) : string.Format("{0:P0}", Share))
                      .ToProperty(this, nameof(Output));

        this.WhenAnyValue(answer => answer.Votes, answer => answer.Share)
            .Do(_ => this.RaisePropertyChanged(nameof(Output)))
            .Subscribe();
    }

    #region Properties

    [Reactive] public IAnswer Model { get; set; }

    public AnswerID ID => Model is null ? AnswerID.None : Model.ID;

    [Reactive] public uint Votes { get; set; }
    public double Share => _share.Value;

    public string Output => _output.Value;

    public ImageSource BarImage => ResourceManager.GetImageSource("ata bar", "jpg", "AtA");

    #endregion Properties
}