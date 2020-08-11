using ReactiveUI;
using WwtbamOld.Model;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для AskTheAudienceView.xaml
    /// </summary>
    public partial class AskTheAudienceView : ReactiveUserControl<AskTheAudience>
    {
        public AskTheAudienceView()
        {
            InitializeComponent();

            imgBackground.ImageSource = ResourceManager.GetImageSource("ata graph 2", "jpg", "AtA");

            this.WhenActivated(d =>
            {
                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

                //d(this.OneWayBind(ViewModel, vm => vm.IsExecuting, v => v.gridMain.Visibility));

                d(this.OneWayBind(ViewModel, vm => vm.Answers, v => v.itemsAnswers.ItemsSource));
            });
        }
    }
}