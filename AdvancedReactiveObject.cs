using ReactiveUI;
using System.Reflection;

namespace WwtbamOld
{
    public abstract class AdvancedReactiveObject : ReactiveObject
    {
        public virtual void UpdateAllProperties()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
                if (propertyInfo.CanRead)
                    this.RaisePropertyChanged(propertyInfo.Name);
        }
    }
}