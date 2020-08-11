using ReactiveUI;
using System.Windows.Controls;
using System.Windows.Input;
using WwtbamOld.ViewModel;

namespace WwtbamOld.View
{
    /// <summary>
    /// Логика взаимодействия для ScreenView.xaml
    /// </summary>
    public partial class ScreenView : ReactiveWindow<ScreenViewModel>
    {
        public ScreenView()
        {
            InitializeComponent();

            imgBackground.ImageSource = ResourceManager.GetImageSource("bg main", "jpg", "Backgrounds");
            imgLogo.Source = ResourceManager.GetImageSource("bg logo", "png", "Backgrounds");
            imgPhotoBigBackground.ImageSource = ResourceManager.GetImageSource("bg photo", "jpg", "Backgrounds");

            this.WhenActivated(d =>
            {
                #region Общее

                d(this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.DataContext));

                d(this.OneWayBind(ViewModel, vm => vm.WindowTitle, v => v.Title));

                d(this.Bind(ViewModel, vm => vm.CoordinateX, v => v.Left));
                d(this.Bind(ViewModel, vm => vm.CoordinateY, v => v.Top));

                #endregion Общее

                #region Lozenge

                d(this.OneWayBind(ViewModel, vm => vm.Lozenge, v => v.lozenge.ViewModel));

                #endregion Lozenge

                #region Small Money Tree

                d(this.OneWayBind(ViewModel, vm => vm.SmallMoneyTree.IsShown, v => v.gridSmallMoneyTree.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Collection, v => v.itemsSmtLifelineIcons.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.SmallMoneyTree.Image, v => v.imgSmallMoneyTree.Source));

                #endregion Small Money Tree

                #region Current Sum

                d(this.OneWayBind(ViewModel, vm => vm.CurrentSum.IsShown, v => v.imgCurrentSum.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.CurrentSum.Image, v => v.imgCurrentSum.Source));

                #endregion Current Sum

                #region Winnings

                d(this.OneWayBind(ViewModel, vm => vm.Winnings.IsShown, v => v.imgWinnings.Visibility));
                d(this.OneWayBind(ViewModel, vm => vm.Winnings.Image, v => v.imgWinnings.Source));

                #endregion Winnings

                #region Big Money Tree

                d(this.OneWayBind(ViewModel, vm => vm.BigMoneyTree, v => v.gridBigMoneyTree.DataContext));
                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Collection, v => v.itemsBmtLifelineIcons.ItemsSource));
                d(this.OneWayBind(ViewModel, vm => vm.BigMoneyTree.Image, v => v.imgBigMoneyTree.Source));

                #endregion Big Money Tree

                #region Lifelines

                d(this.OneWayBind(ViewModel, vm => vm.Lifelines.Selected, v => v.controlLifeline.ViewModel));

                #endregion Lifelines

                #region Big Photo

                d(this.OneWayBind(ViewModel, vm => vm.Photo.IsBigShown, v => v.gridPhotoBig.Visibility));

                #endregion Big Photo

                #region Big Photo

                d(this.OneWayBind(ViewModel, vm => vm.Photo.IsSmallShown, v => v.gridPhotoSmall.Visibility));

                #endregion Big Photo

                #region Logo

                d(this.OneWayBind(ViewModel, vm => vm.IsLogoShown, v => v.imgLogo.Visibility));

                #endregion Logo
            });
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}