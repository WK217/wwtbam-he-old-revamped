using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для FiftyFiftyView.xaml
    /// </summary>
    public partial class FiftyFiftyPanelView : ReactiveUserControl<FiftyFiftyViewModel>
    {
        public FiftyFiftyPanelView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.IsUsable, v => v.groupMain.IsEnabled));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.groupMain.Header));

                d(this.OneWayBind(ViewModel, vm => vm.Alternatives, v => v.comboAlternative.ItemsSource));

                d(this.OneWayBind(ViewModel, vm => vm.CorrectID, v => v.btnCorrect.Content));
                d(this.Bind(ViewModel, vm => vm.Alternative, v => v.comboAlternative.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.ActivateCommand, v => v.btnActivate));
            });
        }
    }
}