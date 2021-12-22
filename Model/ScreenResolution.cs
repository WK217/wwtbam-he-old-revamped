using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace WwtbamOld.Model;

public sealed class ScreenResolution : ReactiveObject, IEquatable<ScreenResolution>
{
    #region Fields

    private readonly ObservableAsPropertyHelper<bool> _isSelected;

    #endregion Fields

    public ScreenResolution(int width, int height, IObservable<ScreenResolution> selected)
    {
        Width = width;
        Height = height;

        _isSelected = selected.Select(r => r.Equals(this))
                              .ToProperty(this, nameof(IsSelected));
    }

    #region Properties

    public int Width { get; }
    public int Height { get; }

    public bool IsSelected => _isSelected.Value;

    public string FriendlyName => $"{Width} × {Height}";

    #endregion Properties

    #region Methods

    public bool Equals(ScreenResolution other) => Width == other.Width && Height == other.Height;

    public override bool Equals(object obj) => obj is ScreenResolution other && Equals(other);

    public override string ToString() => FriendlyName;

    #endregion Methods
}