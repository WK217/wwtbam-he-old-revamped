using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WwtbamOld.Media.Audio;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class HostViewModel : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly MainViewModel _mainViewModel;

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

        IConnectableObservable<ScreenResolution> selected = this.WhenAnyValue(x => x._mainViewModel.Screen.Resolution).Publish();
        ScreenResolutions = new ReadOnlyCollection<ScreenResolution>(new List<ScreenResolution>()
        {
            new ScreenResolution(1366, 768, selected),
            new ScreenResolution(1600, 900, selected),
            new ScreenResolution(1920, 1080, selected)
        });
        selected.Connect();

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

        SetScreenResolutionCommand ??= ReactiveCommand.Create<ScreenResolution>(r => _mainViewModel.Screen.Resolution = r);

        #endregion Commands
    }

    #region Properties

    public string WindowTitle => App.GetWindowTitle("Ведущий");

    public ReadOnlyObservableCollection<Level> Levels => _game.Levels.Collection;
    public IReadOnlyList<ScreenResolution> ScreenResolutions { get; }

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

    public LevelObject BigMoneyTree => _game.BigMoneyTree;

    [Reactive] public bool HasWalkedAway { get; set; }

    [Reactive] public bool IsLogoShown { get; set; }

    public LevelObject Winnings => _game.Winnings;

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

    public QuizbaseViewModel Quizbase { get; }
    public LozengeViewModel Lozenge => _mainViewModel.Lozenge;
    public LifelineTypesViewModel LifelineTypes { get; }
    public LifelinesViewModel Lifelines { get; }

    public MediaViewModel Media { get; }
    public SoundOutDevicesViewModel SoundOutDevices { get; }

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

    public ReactiveCommand<ScreenResolution, Unit> SetScreenResolutionCommand { get; }

    #endregion Commands
}