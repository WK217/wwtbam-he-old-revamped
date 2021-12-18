using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Media;
using WwtbamOld.Media.Audio;

namespace WwtbamOld.Model;

public abstract class Lifeline : ReactiveObject
{
    #region Abstract Properties

    public abstract byte ID { get; }
    public abstract string Code { get; }
    public abstract string Name { get; }
    public virtual Audio PingSound => (Audio)((byte)Audio.Ping1 + Math.Max(Math.Min(_game.Lifelines.Collection.IndexOf(this), 3), 0));

    #endregion Abstract Properties

    #region Fields

    protected readonly Game _game;
    private readonly ObservableAsPropertyHelper<bool> _isUsable;
    protected readonly bool _midIconVisibility;
    private readonly ObservableAsPropertyHelper<bool> _isMidIconShown;

    protected LifelineState _state;
    protected bool _isSecret = true;
    private readonly ObservableAsPropertyHelper<ImageSource> _generalImage;

    private IObservable<long> _pingTimerObservable;

    #endregion Fields

    public Lifeline(Game game, bool midIconVisibility = false)
    {
        _game = game;
        _midIconVisibility = midIconVisibility;

        _isUsable = this.WhenAnyValue(lifeline => lifeline._game.Lifelines.Selected)
                        .Select(selected => selected == this || selected is null)
                        .ToProperty(this, nameof(IsUsable));

        _isMidIconShown = this.WhenAnyValue(lifeline => lifeline.IsExecuting, lifeline => lifeline.State, (flag, state) => flag && _midIconVisibility)
                              .ToProperty(this, nameof(IsMidIconShown));

        _generalImage = this.WhenAnyValue(lifeline => lifeline.State)
                            .Select(state => GetImage(LifelineImageType.General, State))
                            .ToProperty(this, nameof(GeneralImage));
    }

    #region Properties

    public bool IsUsable => _isUsable is not null && _isUsable.Value;

    public virtual bool IsEnabled
    {
        get => _state != LifelineState.Disabled;
        set
        {
            if (IsEnabled != value)
            {
                State = value ? LifelineState.Enabled : LifelineState.Disabled;
                this.RaisePropertyChanged(nameof(IsEnabled));
            }
        }
    }

    public bool IsSecret
    {
        get => _isSecret;
        set
        {
            if (_isSecret != value)
            {
                _isSecret = value;

                this.RaisePropertyChanged(nameof(IsSecret));
                this.RaisePropertyChanged(nameof(State));
            }
        }
    }

    public LifelineState State
    {
        get => IsSecret ? LifelineState.Secret : _state;
        set
        {
            if (State != value)
            {
                _state = value;

                this.RaisePropertyChanged(nameof(State));
                this.RaisePropertyChanged(nameof(IsEnabled));
            }
        }
    }

    [Reactive] public bool IsExecuting { get; set; }
    public bool IsMidIconShown => _isMidIconShown.Value;

    public virtual IObservable<bool> CanActivate => Observable.Return(true);

    public ImageSource GeneralImage => _generalImage.Value;

    public ImageSource MiddleImage => GetImage(LifelineImageType.General, LifelineState.Enabled);

    #endregion Properties

    #region Methods

    public void Ping()
    {
        IsSecret = false;
        State = LifelineState.Activated;
        AudioManager.Play(PingSound);

        _pingTimerObservable = Observable.Timer(TimeSpan.FromMilliseconds(300), RxApp.MainThreadScheduler);
        _pingTimerObservable.Subscribe(v => { State = LifelineState.Enabled; });
    }

    public abstract void Activate();

    private ImageSource GetImage(LifelineImageType type, LifelineState state)
    {
        string folders = "Lifelines";
        string fileName = string.Empty;
        string extension = "png";

        if (type == LifelineImageType.General)
        {
            if (state == LifelineState.Secret)
                fileName = "secret";
            else
                fileName = $"{Code.ToLower()} {state.ToString().ToLower()}";
        }
        else
        {
            folders = Path.Combine(folders, type.ToString().ToLower());
            fileName = $"{(state == LifelineState.Secret ? "secret" : $"{Code.ToLower()} {state.ToString().ToLower()}")} {type.ToString().ToLower()}";
        }

        return ResourceManager.GetImageSource(fileName, extension, folders);
    }

    #endregion Methods
}
