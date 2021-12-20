using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View;

/// <summary>
/// Логика взаимодействия для LozengeView.xaml
/// </summary>
public partial class LozengeView : ReactiveUserControl<LozengeViewModel>
{
    public LozengeView()
    {
        InitializeComponent();

        imgBackground.ImageSource = ResourceManager.GetImageSource("lozenge question", "png", "Lozenge");
        imgLifelinesPanel.ImageSource = ResourceManager.GetImageSource("lifelines panel", "png", "Lifelines");

        this.WhenActivated(d =>
        {
            d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

            d(this.OneWayBind(ViewModel, vm => vm.Model.Question, v => v.txtQuestion.Text));

            d(this.OneWayBind(ViewModel, vm => vm.LifelinesPanel, v => v.gridLifelinesPanel.DataContext));
            d(this.OneWayBind(ViewModel, vm => vm.LifelinesPanel.Lifelines, v => v.itemsLifelinesPanel.ItemsSource));

            d(this.OneWayBind(ViewModel, vm => vm.IsShown, v => v.Visibility));

            d(this.OneWayBind(ViewModel, vm => vm.A, v => v.answerA.ViewModel));
            d(this.OneWayBind(ViewModel, vm => vm.B, v => v.answerB.ViewModel));
            d(this.OneWayBind(ViewModel, vm => vm.C, v => v.answerC.ViewModel));
            d(this.OneWayBind(ViewModel, vm => vm.D, v => v.answerD.ViewModel));

            d(this.OneWayBind(ViewModel, vm => vm.ActivatedLifeline, v => v.lifelineIconMiddle.DataContext));
        });
    }
}