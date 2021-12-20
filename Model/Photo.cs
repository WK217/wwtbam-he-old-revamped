using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class Photo : ReactiveObject
{
    #region Fields

    private readonly Game _game;
    private readonly ObservableAsPropertyHelper<Uri> _photoUrl;
    private readonly ObservableAsPropertyHelper<bool> _canShowImage;

    #endregion Fields

    public Photo(Game game)
    {
        _game = game;

        this.WhenAnyValue(x => x._game.Lozenge.Photo)
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.PhotoUrlString);

        _photoUrl = this.WhenAnyValue(x => x.PhotoUrlString)
                        .Select(url => string.IsNullOrWhiteSpace(url) ? null : new Uri(url))
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .ToProperty(this, nameof(PhotoUrl));

        _canShowImage = this.WhenAnyValue(x => x.PhotoUrl)
                            .Select(url => url is not null)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .ToProperty(this, nameof(CanShowImage));

        this.WhenAnyValue(x => x.CanShowImage)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (!x)
                {
                    IsBigShown = false;
                    IsSmallShown = false;
                }
            });
    }

    #region Properties

    [Reactive] public string PhotoUrlString { get; set; }
    public Uri PhotoUrl => _photoUrl.Value;

    public bool CanShowImage => _canShowImage.Value;

    [Reactive] public bool IsBigShown { get; set; }
    [Reactive] public bool IsSmallShown { get; set; }

    #endregion Properties
}