using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel;

public sealed class LifelinesViewModel : ReadOnlyReactiveCollection<ILifelineViewModel>
{
    #region Fields

    private readonly Lifelines _lifelines;

    private readonly ObservableAsPropertyHelper<bool> _isNotEmpty;

    #endregion Fields

    public LifelinesViewModel(Lifelines lifelines)
    {
        _lifelines = lifelines;
        IConnectableObservable<IChangeSet<Lifeline>> collectionObservable = _lifelines.Collection.ToObservableChangeSet().Publish();
        collectionObservable.Transform(x => CreateLifelineViewModel(x))
                            .Bind(_collection)
                            .Subscribe();
        _isNotEmpty = collectionObservable.ToCollection()
                                          .Select(x => x.Count > 0)
                                          .ToProperty(this, nameof(IsNotEmpty));
        collectionObservable.Connect();

        this.WhenAnyValue(vm => vm.Selected)
            .Select(x => x?.Model)
            .BindTo(_lifelines, x => x.Selected);

        this.WhenAnyValue(vm => vm._lifelines.Selected)
            .Select(x => ViewModels.FirstOrDefault(vm => vm.Model == x))
            .BindTo(this, x => x.Selected);
    }

    #region Properties

    public ReadOnlyObservableCollection<ILifelineViewModel> ViewModels => _readOnlyCollection;
    [Reactive] public ILifelineViewModel Selected { get; set; }

    public bool IsNotEmpty => _isNotEmpty.Value;

    #endregion Properties

    #region Methods

    private ILifelineViewModel CreateLifelineViewModel(Lifeline lifeline)
    {
        return lifeline switch
        {
            FiftyFifty fifty => new FiftyFiftyViewModel(this, fifty),
            PhoneAFriend phone => new TimerLifelineViewModel(this, phone),
            AskTheAudience ata => new AskTheAudienceViewModel(this, ata),
            DoubleDip dd => new DoubleDipViewModel(this, dd),
            Google google => new TimerLifelineViewModel(this, google),
            _ => new AskTheHostViewModel(this, (AskTheHost)lifeline),
        };
    }

    #endregion Methods
}
