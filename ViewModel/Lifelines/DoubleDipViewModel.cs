using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class DoubleDipViewModel : LifelineViewModel<DoubleDip>
{
    #region Fields

    private readonly ObservableCollectionExtended<DoubleDipModeViewModel> _modesCollection;
    private readonly ReadOnlyObservableCollection<DoubleDipModeViewModel> _readOnlyModesCollection;

    #endregion Fields

    public DoubleDipViewModel(LifelinesViewModel lifelinesViewModel, DoubleDip lifeline)
        : base(lifelinesViewModel, lifeline)
    {
        _modesCollection = new ObservableCollectionExtended<DoubleDipModeViewModel>();
        _modesCollection.ToObservableChangeSet()
                        .Bind(out _readOnlyModesCollection)
                        .Subscribe();

        _modesCollection.Add(new DoubleDipModeViewModel(this, DoubleDipMode.Deactivated));
        _modesCollection.Add(new DoubleDipModeViewModel(this, DoubleDipMode.FirstAnswer));
        _modesCollection.Add(new DoubleDipModeViewModel(this, DoubleDipMode.SecondAnswer));

        this.WhenAnyValue(x => x._model.Mode).BindTo(this, x => x.Mode);
        this.WhenAnyValue(x => x.Mode).BindTo(_model, x => x.Mode);

        SetModeCommand = ReactiveCommand.Create<DoubleDipMode>(mode => _model.SetMode(mode));
        DeactivateCommand = ReactiveCommand.Create(() => _model.Deactivate(), _model.CanDeactivate);
    }

    #region Properties

    [Reactive] public DoubleDipMode Mode { get; set; }
    public ReadOnlyObservableCollection<DoubleDipModeViewModel> Modes => _readOnlyModesCollection;

    #endregion Properties

    #region Commands

    public ReactiveCommand<DoubleDipMode, Unit> SetModeCommand { get; }
    public ReactiveCommand<Unit, Unit> DeactivateCommand { get; }

    #endregion Commands
}
