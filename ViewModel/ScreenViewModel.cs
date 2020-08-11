using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class ScreenViewModel : ViewModelBase
    {
        public string WindowTitle => string.Join(" :: ", ((App)Application.Current).AppName, "Экран");

        #region Fields

        private readonly Game _game;
        private readonly ObservableAsPropertyHelper<bool> _isLogoShown;

        #endregion Fields

        public ScreenViewModel(Game game, MainViewModel mainViewModel)
        {
            _game = game;
            _mainViewModel = mainViewModel;

            this.WhenAnyValue(screen => screen._mainViewModel.Host.IsLogoShown).ToProperty(this, nameof(IsLogoShown), out _isLogoShown);

            this.WhenAnyValue(screen => screen._mainViewModel.Host.ScreenX)
                .BindTo(this, x => x.CoordinateX);
            this.WhenAnyValue(screen => screen.CoordinateX)
                .BindTo(_mainViewModel.Host, x => x.ScreenX);

            this.WhenAnyValue(screen => screen._mainViewModel.Host.ScreenY)
                .BindTo(this, x => x.CoordinateY);
            this.WhenAnyValue(screen => screen.CoordinateY)
                .BindTo(_mainViewModel.Host, x => x.ScreenY);
        }

        #region Properties

        public LevelObject SmallMoneyTree => _game.SmallMoneyTree;
        public LevelObject BigMoneyTree => _game.BigMoneyTree;
        public LevelObject CurrentSum => _game.CurrentSum;
        public LevelObject Winnings => _game.Winnings;

        public Lifelines Lifelines => _game.Lifelines;

        public bool IsLogoShown => _isLogoShown.Value;

        public Photo Photo => _mainViewModel.Host.Photo;

        [Reactive] public double CoordinateX { get; set; }
        [Reactive] public double CoordinateY { get; set; }

        #endregion Properties

        #region View Models

        private readonly MainViewModel _mainViewModel;
        public LozengeViewModel Lozenge => _mainViewModel.Lozenge;

        #endregion View Models
    }
}