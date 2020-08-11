using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class LifelinesViewModel : ReadOnlyReactiveCollection<ILifelineViewModel>
    {
        #region Fields

        private readonly Lifelines _lifelines;

        #endregion Fields

        public LifelinesViewModel(Lifelines lifelines)
        {
            _lifelines = lifelines;
            _lifelines.Collection.ToObservableChangeSet()
                                 .Transform(x => CreateLifelineViewModel(x))
                                 .Bind(_collection)
                                 .Subscribe();

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

        #endregion Properties

        #region Methods

        private ILifelineViewModel CreateLifelineViewModel(Lifeline lifeline)
        {
            return lifeline switch
            {
                FiftyFifty fifty => new FiftyFiftyViewModel(this, fifty),
                PhoneAFriend phone => new PhoneAFriendViewModel(this, phone),
                AskTheAudience ata => new AskTheAudienceViewModel(this, ata),
                DoubleDip dd => new DoubleDipViewModel(this, dd),
                _ => new AskTheHostViewModel(this, (AskTheHost)lifeline),
            };
        }

        #endregion Methods
    }
}