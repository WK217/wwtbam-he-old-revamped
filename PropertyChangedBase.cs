using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WwtbamOld;

public abstract class PropertyChangedBase : INotifyPropertyChanged
{
    protected T RaiseAndSetIfChanged<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            return newValue;

        backingField = newValue;
        RaisePropertyChanged(propertyName);
        return newValue;
    }

    public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChangedEventHandler propertyChanged = PropertyChanged;
        if (propertyChanged is null)
            return;

        propertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual void UpdateAllProperties()
    {
        foreach (PropertyInfo propertyInfo in GetType().GetProperties())
        {
            if (propertyInfo.CanRead)
                RaisePropertyChanged(propertyInfo.Name);
        }
    }
}