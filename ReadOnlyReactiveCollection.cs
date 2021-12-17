using DynamicData.Binding;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WwtbamOld
{
    public abstract class ReadOnlyReactiveCollection<T> : ReactiveObject, IEnumerable<T>
    {
        #region Fields

        protected readonly ObservableCollectionExtended<T> _collection;
        protected readonly ReadOnlyObservableCollection<T> _readOnlyCollection;

        #endregion Fields

        public ReadOnlyReactiveCollection()
        {
            _readOnlyCollection = new ReadOnlyObservableCollection<T>(_collection = new ObservableCollectionExtended<T>());
        }

        #region Properties

        public ReadOnlyObservableCollection<T> Collection => _readOnlyCollection;

        #endregion Properties

        public T this[int id] => _readOnlyCollection[id];

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_readOnlyCollection).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_readOnlyCollection).GetEnumerator();

        #endregion IEnumerable<T>
    }
}