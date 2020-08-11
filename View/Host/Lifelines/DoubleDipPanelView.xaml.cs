using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для DoubleDipView.xaml
    /// </summary>
    public partial class DoubleDipPanelView : ReactiveUserControl<DoubleDipViewModel>
    {
        public DoubleDipPanelView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.IsUsable, v => v.groupMain.IsEnabled));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.groupMain.Header));

                d(this.OneWayBind(ViewModel, vm => vm.Modes, v => v.itemsModes.ItemsSource));
                d(this.BindCommand(ViewModel, vm => vm.ActivateCommand, v => v.btnActivate));
                d(this.BindCommand(ViewModel, vm => vm.DeactivateCommand, v => v.btnDeactivate));
            });
        }
    }
}