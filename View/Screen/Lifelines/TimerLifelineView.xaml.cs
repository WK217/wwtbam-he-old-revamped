using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для PhoneAFriendView.xaml
    /// </summary>
    public partial class TimerLifelineView : ReactiveUserControl<TimerLifelineViewModel>
    {
        public TimerLifelineView()
        {
            InitializeComponent();

            imgPhone.Source = ResourceManager.GetImageSource("timer graph 0", "png", "Phone Timer");

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

                //d(this.OneWayBind(ViewModel, vm => vm.IsExecuting, v => v.gridMain.Visibility));

                d(this.OneWayBind(ViewModel, vm => vm.Progress, v => v.progressCountdown.Value));
                d(this.OneWayBind(ViewModel, vm => (uint)vm.RemainingTime.TotalSeconds, v => v.txtTimer.Text));
            });
        }
    }
}