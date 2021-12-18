using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using WwtbamOld.Media.Audio;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class HostViewModel : ViewModelBase
{
    public string WindowTitle => string.Join(" :: ", ((App)Application.Current).AppName, "Ведущий");

    #region Fields

    private readonly Game _game;

    #endregion Fields

    public HostViewModel(Game game, MainViewModel mainViewModel)
    {
        _game = game;
        _mainViewModel = mainViewModel;

        Quizbase = new QuizbaseViewModel(_game.Quizbase);

        LifelineTypes = new LifelineTypesViewModel(_game);
        Lifelines = new LifelinesViewModel(_game.Lifelines);

        Media = new MediaViewModel(this);
        SoundOutDevices = new SoundOutDevicesViewModel();

        Photo = new Photo(_game);

        this.WhenAnyValue(vm => vm._game.HasWalkedAway).BindTo(this, x => x.HasWalkedAway);
        this.WhenAnyValue(vm => vm.HasWalkedAway).BindTo(_game, x => x.HasWalkedAway);

        #region Commands

        LightsDownCommand = ReactiveCommand.Create(() => _game.LightsDown());
        ShowBigTreeCommand = ReactiveCommand.Create(() => { BigMoneyTree.IsShown = !BigMoneyTree.IsShown; });
        AskQuestionCommand = ReactiveCommand.Create(() => _game.ShowQuestion());
        ShowAnswersCommand = ReactiveCommand.Create(() => _game.Lozenge.ShowAnswers());
        WalkawayCommand = ReactiveCommand.Create(() => _game.Walkaway());

        LockACommand = ReactiveCommand.Create(() => _game.LockAnswer(AnswerID.A), this.WhenAnyValue(vm => vm.Lozenge.A.IsShown));
        LockBCommand = ReactiveCommand.Create(() => _game.LockAnswer(AnswerID.B), this.WhenAnyValue(vm => vm.Lozenge.B.IsShown));
        LockCCommand = ReactiveCommand.Create(() => _game.LockAnswer(AnswerID.C), this.WhenAnyValue(vm => vm.Lozenge.C.IsShown));
        LockDCommand = ReactiveCommand.Create(() => _game.LockAnswer(AnswerID.D), this.WhenAnyValue(vm => vm.Lozenge.D.IsShown));
        RevealCorrectCommand = ReactiveCommand.Create(() => _game.RevealCorrect(), this.WhenAnyValue(vm => vm._game.Lozenge.Locked).Select(v => v != AnswerID.None));

        PlayAudioCommand = ReactiveCommand.Create<Audio>(audio => AudioManager.Play(audio));

        #endregion Commands
    }

    #region Properties

    public ReadOnlyObservableCollection<Level> Levels => _game.Levels.Collection;

    public Level CurrentLevel
    {
        get => _mainViewModel.CurrentLevel;
        set
        {
            if (_mainViewModel.CurrentLevel != value)
            {
                _mainViewModel.CurrentLevel = value;
                this.RaisePropertyChanged(nameof(CurrentLevel));
            }
        }
    }

    public Quiz CurrentQuiz
    {
        get => _mainViewModel.CurrentQuiz;
        set
        {
            if (_mainViewModel.CurrentQuiz != value)
            {
                _mainViewModel.CurrentQuiz = value;
                this.RaisePropertyChanged(nameof(CurrentQuiz));
            }
        }
    }

    public LevelObject BigMoneyTree => _game.BigMoneyTree;

    [Reactive] public bool HasWalkedAway { get; set; }

    [Reactive] public bool IsLogoShown { get; set; }

    public LevelObject Winnings => _game.Winnings;

    public SoundOutDevicesViewModel SoundOutDevices { get; }

    public Photo Photo { get; }

    [Reactive] public double ScreenX { get; set; }
    [Reactive] public double ScreenY { get; set; }

    [Reactive] public bool SpoilerFree { get; set; }

    #endregion Properties

    #region Methods

    public void ClearScreen()
    {
        _game.Clear();
    }

    #endregion Methods

    #region View Models

    private readonly MainViewModel _mainViewModel;

    public QuizbaseViewModel Quizbase { get; }
    public LozengeViewModel Lozenge => _mainViewModel.Lozenge;
    public LifelineTypesViewModel LifelineTypes { get; }
    public LifelinesViewModel Lifelines { get; }

    public MediaViewModel Media { get; }

    #endregion View Models

    #region Commands

    public ReactiveCommand<Unit, Unit> LightsDownCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowBigTreeCommand { get; }
    public ReactiveCommand<Unit, Unit> AskQuestionCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowAnswersCommand { get; }
    public ReactiveCommand<Unit, Unit> WalkawayCommand { get; }

    public ReactiveCommand<Unit, Unit> LockACommand { get; }
    public ReactiveCommand<Unit, Unit> LockBCommand { get; }
    public ReactiveCommand<Unit, Unit> LockCCommand { get; }
    public ReactiveCommand<Unit, Unit> LockDCommand { get; }
    public ReactiveCommand<AnswerID, Unit> LockAnswerCommand { get; }
    public ReactiveCommand<Unit, Unit> RevealCorrectCommand { get; }

    public ReactiveCommand<FiftyFiftyViewModel, Unit> ShowFiftyFiftyWindowCommand { get; }

    public ReactiveCommand<Audio, Unit> PlayAudioCommand { get; }

    #endregion Commands
}
