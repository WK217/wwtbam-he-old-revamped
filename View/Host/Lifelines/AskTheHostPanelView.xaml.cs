using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для AskTheHostPanelView.xaml
    /// </summary>
    public partial class AskTheHostPanelView : ReactiveUserControl<AskTheHostViewModel>
    {
        public AskTheHostPanelView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.IsUsable, v => v.groupMain.IsEnabled));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.groupMain.Header));

                d(this.BindCommand(ViewModel, vm => vm.ActivateCommand, v => v.btnActivate));
                d(this.BindCommand(ViewModel, vm => vm.DeactivateCommand, v => v.btnDeactivate));
            });
        }
    }
}