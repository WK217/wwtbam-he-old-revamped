using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для PhoneAFriendView.xaml
    /// </summary>
    public partial class TimerLifelinePanelView : ReactiveUserControl<TimerLifelineViewModel>
    {
        public TimerLifelinePanelView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.IsUsable, v => v.groupMain.IsEnabled));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.groupMain.Header));

                d(this.Bind(ViewModel, vm => vm.IsTimerShown, v => v.checkTimerShown.IsChecked));
                d(this.Bind(ViewModel, vm => vm.Duration, v => v.txtDuration.Text));
                d(this.Bind(ViewModel, vm => vm.Countdown, v => v.txtCountdown.Text));

                d(this.BindCommand(ViewModel, vm => vm.InitiateCommand, v => v.btnInitiate));
                d(this.BindCommand(ViewModel, vm => vm.ActivateCommand, v => v.btnActivate));
                d(this.BindCommand(ViewModel, vm => vm.StopCommand, v => v.btnStop));
            });
        }
    }
}