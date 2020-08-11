using System.Collections.Generic;

namespace WwtbamOld.Model
{
    public sealed class Levels : ReadOnlyReactiveCollection<Level>
    {
        public void Initialize(IEnumerable<Level> collection)
        {
            _collection.Clear();
            _collection.AddRange(collection);
        }
    }
}