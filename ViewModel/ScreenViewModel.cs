using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class ScreenViewModel : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly ObservableAsPropertyHelper<bool> _isLogoShown;

    #endregion Fields

    public ScreenViewModel(Game game, MainViewModel mainViewModel)
    {
        _game = game;
        _mainViewModel = mainViewModel;

        this.WhenAnyValue(screen => screen._mainViewModel.Host.IsLogoShown)
            .ToProperty(this, nameof(IsLogoShown), out _isLogoShown);

        this.WhenAnyValue(screen => screen._mainViewModel.Host.ScreenX)
            .BindTo(this, x => x.CoordinateX);
        this.WhenAnyValue(screen => screen.CoordinateX)
            .BindTo(_mainViewModel.Host, x => x.ScreenX);

        this.WhenAnyValue(screen => screen._mainViewModel.Host.ScreenY)
            .BindTo(this, x => x.CoordinateY);
        this.WhenAnyValue(screen => screen.CoordinateY)
            .BindTo(_mainViewModel.Host, x => x.ScreenY);

        Resolution = _mainViewModel.Host?.ScreenResolutions[^1];
    }

    #region Properties

    public string WindowTitle => App.GetWindowTitle("Экран");

    public LevelObject SmallMoneyTree => _game.SmallMoneyTree;
    public LevelObject BigMoneyTree => _game.BigMoneyTree;
    public LevelObject CurrentSum => _game.CurrentSum;
    public LevelObject Winnings => _game.Winnings;

    public LifelinesViewModel Lifelines => _mainViewModel.Host.Lifelines;

    public bool IsLogoShown => _isLogoShown.Value;

    public Photo Photo => _mainViewModel.Host.Photo;

    [Reactive] public double CoordinateX { get; set; }
    [Reactive] public double CoordinateY { get; set; }

    [Reactive] public ScreenResolution Resolution { get; set; }

    #endregion Properties

    #region View Models

    private readonly MainViewModel _mainViewModel;
    public LozengeViewModel Lozenge => _mainViewModel.Lozenge;

    #endregion View Models
}