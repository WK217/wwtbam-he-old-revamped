using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using WwtbamOld.Interactions;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class QuizbaseViewModel : ReactiveObject
    {
        #region Fields

        private readonly Quizbase _quizbase;

        #endregion Fields

        public QuizbaseViewModel(Quizbase quizbase)
        {
            _quizbase = quizbase;
            SelectedQuiz = _quizbase.SelectedQuiz;
            this.WhenAnyValue(vm => vm._quizbase.SelectedQuiz)
                .Where(quiz => quiz != null)
                .BindTo(this, quizbase => quizbase.SelectedQuiz);
            this.WhenAnyValue(vm => vm.SelectedQuiz)
                .Where(quiz => quiz != null)
                .BindTo(_quizbase, quizbase => quizbase.SelectedQuiz);

            #region Commands

            LoadDefaultQuizbaseCommand = ReactiveCommand.Create(() => LoadQuizbase(ResourceManager.LoadQuizzesDefault()));

            LoadQuizbaseFileCommand = ReactiveCommand.CreateFromObservable(() => DialogWindowInteractions.ShowOpenQuizbaseDialog.Handle(Unit.Default));
            LoadQuizbaseFileCommand.Subscribe(value => LoadQuizbase(value));

            #endregion Commands
        }

        #region Properties

        public ReadOnlyObservableCollection<Quiz> Quizbase => _quizbase.Collection;
        [Reactive] public Quiz SelectedQuiz { get; set; }

        #endregion Properties

        #region Methods

        private void LoadQuizbase(IEnumerable<Quiz> quizzes)
        {
            _quizbase.Initialize(quizzes);
            if (_quizbase.Collection.Count > 0)
                SelectedQuiz = _quizbase[0];
        }

        private void LoadQuizbase(Quiz quiz)
        {
            _quizbase.Initialize(quiz);
            if (_quizbase.Collection.Count > 0)
                SelectedQuiz = _quizbase[0];
        }

        #endregion Methods

        #region Commands

        public ReactiveCommand<Unit, Unit> LoadDefaultQuizbaseCommand { get; }
        public ReactiveCommand<Unit, IEnumerable<Quiz>> LoadQuizbaseFileCommand { get; }

        #endregion Commands
    }
}