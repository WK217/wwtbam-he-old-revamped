using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для AskTheAudienceView.xaml
    /// </summary>
    public partial class AskTheAudiencePanelView : ReactiveUserControl<AskTheAudienceViewModel>
    {
        public AskTheAudiencePanelView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.IsUsable, v => v.groupMain.IsEnabled));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.groupMain.Header));

                d(this.BindCommand(ViewModel, vm => vm.InitiateCommand, v => v.btnInitiate));
                d(this.BindCommand(ViewModel, vm => vm.EndCommand, v => v.btnEnd));

                d(this.Bind(ViewModel, vm => vm.IsTableShown, v => v.checkTableShown.IsChecked));

                d(this.OneWayBind(ViewModel, vm => vm.Answers, v => v.itemsAnswers.ItemsSource));

                d(this.Bind(ViewModel, vm => vm.AreResultsShown, v => v.checkResultsShown.IsChecked));

                d(this.OneWayBind(ViewModel, vm => vm.DataTypes, v => v.comboDataType.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.DataType, v => v.comboDataType.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.ActivateCommand, v => v.btnActivate));
                d(this.BindCommand(ViewModel, vm => vm.DeactivateCommand, v => v.btnDeactivate));
            });
        }
    }
}