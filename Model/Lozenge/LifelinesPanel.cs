using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace WwtbamOld.Model;

public sealed class LifelinesPanel : ReactiveObject
{
    #region Fields

    private readonly Model.Lifelines _lifelines;

    #endregion Fields

    public LifelinesPanel(Lifelines lifelines)
    {
        _lifelines = lifelines;
    }

    #region Properties

    public ReadOnlyObservableCollection<Lifeline> Lifelines => _lifelines.Collection;
    [Reactive] public bool IsShown { get; set; }

    #endregion Properties
}
