using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace WwtbamOld.View;

public sealed class ObjectToDescriptionTypeConverter : IValueConverter, IBindingTypeConverter
{
    #region IValueConverter

    public object Convert(object value,
                          Type targetType,
                          object parameter,
                          CultureInfo culture)
    {
        if (value is Type type)
            return GetDescription(type);
        else
            return value is null ? Binding.DoNothing : GetDescription(value);
    }

    public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
    {
        return Binding.DoNothing;
    }

    #endregion IValueConverter

    #region IBindingTypeConverter

    /// <inheritdoc/>
    public int GetAffinityForObjects(Type fromType, Type toType)
    {
        if (toType == typeof(string))
            return 10;

        return 0;
    }

    /// <inheritdoc/>
    public bool TryConvert(object from, Type toType, object conversionHint, out object result)
    {
        if (toType == typeof(string))
        {
            result = Convert(from, toType, null, CultureInfo.CurrentCulture);
            return true;
        }

        result = null;
        return false;
    }

    #endregion IBindingTypeConverter

    private string GetDescription(object obj)
    {
        MemberInfo[] member = obj.GetType().GetMember(obj.ToString());

        if (member is not null && member.Length != 0)
        {
            object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes is not null && customAttributes.Length != 0)
                return ((DescriptionAttribute)customAttributes[0]).Description;
        }

        return obj.ToString();
    }

    private string GetDescription(Type type)
    {
        DescriptionAttribute[] descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return descriptions.Length == 0 ? string.Empty : descriptions[0].Description;
    }
}

public sealed class SoundOutToSelectedConverter : IValueConverter
{
    public object Convert(object value,
                          Type targetType,
                          object parameter,
                          CultureInfo culture)
    {
        return parameter is IEnumerable<object> collection ? collection.Contains(value) : (object)false;
    }

    public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
