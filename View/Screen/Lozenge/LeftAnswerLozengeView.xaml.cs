using ReactiveUI;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View;

/// <summary>
/// Логика взаимодействия для LeftAnswerLozengeView.xaml
/// </summary>
public partial class LeftAnswerLozengeView : ReactiveUserControl<IAnswerLozengeViewModel>
{
    public LeftAnswerLozengeView()
    {
        InitializeComponent();

        imgBackground.ImageSource = ResourceManager.GetImageSource("lozenge left", "png", "Lozenge");
        imgRhomb.Source = ResourceManager.GetImageSource("lozenge rhomb", "png", "Lozenge");

        imgLocked.ImageSource = ResourceManager.GetImageSource("lozenge left locked", "png", "Lozenge");
        imgRhombLocked.Source = ResourceManager.GetImageSource("lozenge rhomb white", "png", "Lozenge");

        imgCorrect.ImageSource = ResourceManager.GetImageSource("lozenge left correct", "png", "Lozenge");
        imgRhombCorrect.Source = imgRhombLocked.Source;

        this.WhenActivated(d =>
        {
            d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

            d(this.OneWayBind(ViewModel, vm => vm.ID, v => v.idDefault.Text, id => $"{id}:"));
            d(this.OneWayBind(ViewModel, vm => vm.ID, v => v.idLocked.Text, id => $"{id}:"));
            d(this.OneWayBind(ViewModel, vm => vm.ID, v => v.idCorrect.Text, id => $"{id}:"));

            d(this.OneWayBind(ViewModel, vm => vm.Text, v => v.txtDefault.Text));
            d(this.OneWayBind(ViewModel, vm => vm.Text, v => v.txtLocked.Text));
            d(this.OneWayBind(ViewModel, vm => vm.Text, v => v.txtCorrect.Text));
        });
    }
}
