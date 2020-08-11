using ReactiveUI;
using WwtbamOld.Model;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для AskTheAudienceAnswerView.xaml
    /// </summary>
    public partial class AskTheAudienceAnswerView : ReactiveUserControl<AskTheAudienceAnswer>
    {
        public AskTheAudienceAnswerView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

                d(this.OneWayBind(ViewModel, vm => vm.Output, v => v.txtValue.Text));
                d(this.OneWayBind(ViewModel, vm => vm.BarImage, v => v.imgBar.Source));

                d(this.OneWayBind(ViewModel, vm => vm.Share, v => v.imgBar.Height, ShareToBarHeightConverterFunc));
            });
        }

        private double ShareToBarHeightConverterFunc(double share)
            => share * rowBar.Height.Value;
    }
}