using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class AskTheAudienceViewModel : LifelineViewModel<AskTheAudience>
{
    #region Fields

    private readonly ObservableAsPropertyHelper<uint> _votesSum;

    #endregion Fields

    public AskTheAudienceViewModel(LifelinesViewModel lifelinesViewModel, AskTheAudience lifeline)
        : base(lifelinesViewModel, lifeline)
    {
        InitiateCommand = ReactiveCommand.Create(() => _model.Initiate());
        DeactivateCommand = ReactiveCommand.Create(() => _model.Deactivate());
        EndCommand = ReactiveCommand.Create(() => _model.End());

        this.WhenAnyValue(vm => vm._model.IsTableShown)
            .BindTo(this, x => x.IsTableShown);
        this.WhenAnyValue(vm => vm.IsTableShown)
            .BindTo(_model, x => x.IsTableShown);

        this.WhenAnyValue(vm => vm._model.DataType)
            .BindTo(this, x => x.DataType);
        this.WhenAnyValue(vm => vm.DataType)
            .BindTo(_model, x => x.DataType);

        _votesSum = this.WhenAnyValue(vm => vm._model.VotesSum)
                        .ToProperty(this, nameof(VotesSum));

        this.WhenAnyValue(vm => vm._model.AreResultsShown)
            .BindTo(this, x => x.AreResultsShown);
        this.WhenAnyValue(vm => vm.AreResultsShown)
            .BindTo(_model, x => x.AreResultsShown);
    }

    #region Properties

    [Reactive] public bool IsTableShown { get; set; }

    public AskTheAudienceAnswer A => _model.A;
    public AskTheAudienceAnswer B => _model.B;
    public AskTheAudienceAnswer C => _model.C;
    public AskTheAudienceAnswer D => _model.D;

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
    public IEnumerable<AskTheAudienceDataType> DataTypes => Enum.GetValues(typeof(AskTheAudienceDataType)).Cast<AskTheAudienceDataType>();

    [Reactive] public bool AreResultsShown { get; set; }

    public uint VotesSum => _votesSum.Value;

    #endregion Properties

    #region Commands

    public ReactiveCommand<Unit, Unit> InitiateCommand { get; }
    public ReactiveCommand<Unit, Unit> DeactivateCommand { get; }
    public ReactiveCommand<Unit, Unit> EndCommand { get; }

    #endregion Commands
}
