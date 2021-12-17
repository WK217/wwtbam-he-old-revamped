using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using WwtbamOld.Model;
using System;

namespace WwtbamOld.ViewModel
{
    [Description("«Помощь ведущего»")]
    public sealed class AskTheHostViewModel : LifelineViewModel<AskTheHost>
    {
        public AskTheHostViewModel(LifelinesViewModel lifelinesViewModel, AskTheHost lifeline)
            : base(lifelinesViewModel, lifeline)
        {
            Alternatives = new ReadOnlyObservableCollection<AnswerID>(
                new ObservableCollectionExtended<AnswerID>(new[] { AnswerID.A, AnswerID.B, AnswerID.C, AnswerID.D }));

            this.WhenAnyValue(vm => vm._model.PlayerAnswer1,
                              vm => vm._model.PlayerAnswer2,
                              vm => vm._model.HostAnswer)
                .Subscribe(_ =>
                {
                    this.RaisePropertyChanged(nameof(PlayerAnswer1));
                    this.RaisePropertyChanged(nameof(PlayerAnswer2));
                    this.RaisePropertyChanged(nameof(HostAnswer));
                });

            VerifyAnswersPlayerCommand = ReactiveCommand.Create(() => _model.VerifyAnswers(), _model.CanVerifyPlayerAnswers);
            VerifyAnswersHostCommand = ReactiveCommand.Create(() => _model.VerifyAnswers(false), _model.CanVerifyHostAnswer);
            DeactivateCommand = ReactiveCommand.Create(() => _model.Deactivate());
        }

        #region Properties

        public ReadOnlyObservableCollection<AnswerID> Alternatives { get; }

        public AnswerID PlayerAnswer1 { get => _model.PlayerAnswer1; set => _model.PlayerAnswer1 = value; }
        public AnswerID PlayerAnswer2 { get => _model.PlayerAnswer2; set => _model.PlayerAnswer2 = value; }
        public AnswerID HostAnswer { get => _model.HostAnswer; set => _model.HostAnswer = value; }

        #endregion Properties

        #region Commands

        public ReactiveCommand<Unit, Unit> VerifyAnswersHostCommand { get; }
        public ReactiveCommand<Unit, Unit> VerifyAnswersPlayerCommand { get; }
        public ReactiveCommand<Unit, Unit> DeactivateCommand { get; }

        #endregion Commands
    }
}