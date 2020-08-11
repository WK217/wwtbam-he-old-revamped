using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
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
            _collection = new ObservableCollectionExtended<T>();
            _collection.ToObservableChangeSet()
                       .Bind(out _readOnlyCollection)
                       .Subscribe();
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