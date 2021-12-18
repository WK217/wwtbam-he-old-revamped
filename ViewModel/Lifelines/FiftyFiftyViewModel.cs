using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    [Description("«50 на 50»")]
    public sealed class FiftyFiftyViewModel : LifelineViewModel<FiftyFifty>
    {
        #region Fields

        private readonly ObservableCollectionExtended<AnswerID> _alternatives;
        private readonly ReadOnlyObservableCollection<AnswerID> _readOnlyAlternatives;

        private readonly ObservableAsPropertyHelper<AnswerID> _correctID;

        #endregion Fields

        public FiftyFiftyViewModel(LifelinesViewModel lifelinesViewModel, FiftyFifty lifeline)
            : base(lifelinesViewModel, lifeline)
        {
            _alternatives = new ObservableCollectionExtended<AnswerID>();
            _alternatives.AddRange(new[] { AnswerID.A, AnswerID.B, AnswerID.C, AnswerID.D });

            var alternativeFilter = this.WhenAnyValue(vm => vm.CorrectID, vm => vm.Alternative)
                                        .Select(AlternativeFilter);
            var comparer = SortExpressionComparer<AnswerID>.Ascending(id => id);
            _alternatives.ToObservableChangeSet()
                         .Filter(alternativeFilter)
                         .Sort(comparer)
                         .ObserveOnDispatcher()
                         .Bind(out _readOnlyAlternatives)
                         .DisposeMany()
                         .Subscribe();

            _correctID = this.WhenAnyValue(vm => vm._model.CorrectID)
                             .ToProperty(this, nameof(CorrectID));

            this.WhenAnyValue(vm => vm._model.Alternative)
                .BindTo(this, vm => vm.Alternative);
            this.WhenAnyValue(vm => vm.Alternative)
                .BindTo(_model, m => m.Alternative);
        }

        private Func<AnswerID, bool> AlternativeFilter((AnswerID correct, AnswerID alternative) arg)
            => answer => (answer != arg.correct || answer == arg.alternative) && answer != AnswerID.None;

        #region Properties

        public AnswerID CorrectID => _correctID is not null ? _correctID.Value : AnswerID.None;
        [Reactive] public AnswerID Alternative { get; set; }
        public ReadOnlyObservableCollection<AnswerID> Alternatives => _readOnlyAlternatives;

        #endregion Properties
    }
}