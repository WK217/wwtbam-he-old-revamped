using System;
using WwtbamOld.Model;

namespace WwtbamOld.ViewModel
{
    public sealed class LifelineTypesViewModel : ReadOnlyReactiveCollection<LifelineTypeViewModel>
    {
        public LifelineTypesViewModel(Game game)
        {
            foreach (Type lifelineType in game.Lifelines.GetAllLifelineTypes())
                _collection.Add(new LifelineTypeViewModel(game, lifelineType));
        }
    }
}