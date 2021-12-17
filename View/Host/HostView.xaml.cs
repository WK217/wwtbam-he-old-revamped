using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для HostView.xaml
    /// </summary>
    public partial class HostView : ReactiveWindow<HostViewModel>
    {
        public HostView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                #region Общее

                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

                d(this.OneWayBind(ViewModel, vm => vm.WindowTitle, v => v.Title));

                d(this.BindCommand(ViewModel, vm => vm.Quizbase.LoadQuizbaseFileCommand, v => v.menuLoadQuizbase));
                d(this.BindCommand(ViewModel, vm => vm.Quizbase.LoadDefaultQuizbaseCommand, v => v.menuDefaultQuizbase));

                d(this.OneWayBind(ViewModel, vm => vm.SoundOutDevices.Collection, v => v.menuSoundOut.ItemsSource));

                d(this.Bind(ViewModel, vm => vm.SpoilerFree, v => v.menuSpoilerFree.IsChecked));

                #endregion Общее

                #region Редактирование текущего вопроса

                d(this.OneWayBind(ViewModel, vm => vm.Quizbase.Quizbase, v => v.comboQuizzes.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.Quizbase.SelectedQuiz, v => v.comboQuizzes.SelectedItem));
                d(this.Bind(ViewModel, vm => vm.CurrentQuiz.Question.Text, v => v.textQuestion.Text));
                d(this.OneWayBind(ViewModel, vm => vm.CurrentQuiz, v => v.itemsAnswers.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.CurrentQuiz.Comment.Text, v => v.textComment.Text));

                d(this.Bind(ViewModel, vm => vm.Photo.PhotoUrl, v => v.textPhotoUrl.Text));
                d(this.Bind(ViewModel, vm => vm.Photo.IsBigShown, v => v.checkBigPhotoShown.IsChecked));
                d(this.Bind(ViewModel, vm => vm.Photo.IsSmallShown, v => v.checkSmallPhotoShown.IsChecked));
                d(this.Bind(ViewModel, vm => vm.Photo.CanShowImage, v => v.checkBigPhotoShown.IsEnabled));
                d(this.Bind(ViewModel, vm => vm.Photo.CanShowImage, v => v.checkSmallPhotoShown.IsEnabled));

                #endregion Редактирование текущего вопроса

                #region Управление игровым процессом

                d(this.OneWayBind(ViewModel, vm => vm.Levels, v => v.comboLevels.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.CurrentLevel, v => v.comboLevels.SelectedItem));

                d(this.OneWayBind(ViewModel, vm => vm.Lozenge.A, v => v.checkShowA.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Lozenge.B, v => v.checkShowB.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Lozenge.C, v => v.checkShowC.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Lozenge.D, v => v.checkShowD.DataContext));

                d(this.BindCommand(ViewModel, vm => vm.LightsDownCommand, v => v.btnLightsDown));
                d(this.BindCommand(ViewModel, vm => vm.ShowBigTreeCommand, v => v.btnShowBigTree));
                d(this.BindCommand(ViewModel, vm => vm.AskQuestionCommand, v => v.btnAskQuestion));
                d(this.BindCommand(ViewModel, vm => vm.ShowAnswersCommand, v => v.btnShowAnswers));

                d(this.BindCommand(ViewModel, vm => vm.LockACommand, v => v.btnLockA));
                d(this.BindCommand(ViewModel, vm => vm.LockBCommand, v => v.btnLockB));
                d(this.BindCommand(ViewModel, vm => vm.LockCCommand, v => v.btnLockC));
                d(this.BindCommand(ViewModel, vm => vm.LockDCommand, v => v.btnLockD));

                d(this.BindCommand(ViewModel, vm => vm.RevealCorrectCommand, v => v.btnRevealCorrect));

                d(this.BindCommand(ViewModel, vm => vm.WalkawayCommand, v => v.btnWalkaway));
                d(this.Bind(ViewModel, vm => vm.HasWalkedAway, v => v.checkWalkedaway.IsChecked));
                d(this.Bind(ViewModel, vm => vm.Winnings.IsShown, v => v.checkWinningsShown.IsChecked));

                #endregion Управление игровым процессом

                #region Управление подсказками

                d(this.OneWayBind(ViewModel, vm => vm.LifelineTypes.Collection, v => v.itemsAllLifelineTypes.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Collection, v => v.itemsLifelines.ItemsSource));

                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.IsNotEmpty, v => v.panelLifelines.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.IsNotEmpty, v => v.panelLifelinesApplication.Visibility));

                d(this.BindCommand(ViewModel, vm => vm.Lozenge.ShowLifelinesPanelCommand, v => v.btnShowLifelinesPanel));

                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Collection, v => v.comboLifelines.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.Lifelines.Selected, v => v.comboLifelines.SelectedItem));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Selected, v => v.viewHostLifeline.Content));

                #endregion Управление подсказками

                #region Управление оформлением

                d(this.Bind(ViewModel, vm => vm.IsLogoShown, v => v.checkLogo.IsChecked));

                d(this.OneWayBind(ViewModel, vm => vm.Media.PreLoop, v => v.btnPreLoop.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.Opening, v => v.btnOpening.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.Meeting, v => v.btnMeeting.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.BackgroundGeneral, v => v.btnBackgroundGeneral.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.RulesExplanation, v => v.btnRulesExplanation.DataContext));

                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.IsNotEmpty, v => v.itemsPingLifelines.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Collection, v => v.itemsPingLifelines.ItemsSource));

                d(this.OneWayBind(ViewModel, vm => vm.Media.CommercialIn, v => v.btnCommIn.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.CommercialOut, v => v.btnCommOut.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.Goodbye, v => v.btnGoodbye.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.FinalSiren, v => v.btnFinalSiren.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Media.ClosingTitles, v => v.btnClosing.DataContext));

                d(this.Bind(ViewModel, vm => vm.ScreenX, v => v.textCoordX.Text));
                d(this.Bind(ViewModel, vm => vm.ScreenY, v => v.textCoordY.Text));

                #endregion Управление оформлением
            });
        }
    }
}