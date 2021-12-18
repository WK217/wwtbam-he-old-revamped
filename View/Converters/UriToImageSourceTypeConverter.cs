using ReactiveUI;
using System;
using System.Windows.Media;

namespace WwtbamOld.View;

public sealed class UriToImageSourceTypeConverter : IBindingTypeConverter
{
    /// <inheritdoc/>
    public int GetAffinityForObjects(Type fromType, Type toType)
    {
        if (fromType == typeof(string) && toType == typeof(ImageSource))
            return 10;

        if (fromType == typeof(ImageSource) && toType == typeof(string))
            return 10;

        return 0;
    }

    /// <inheritdoc/>
    public bool TryConvert(object from, Type toType, object conversionHint, out object result)
    {
        if (toType == typeof(ImageSource))
        {
            string uri = from.ToString();
            result = string.IsNullOrWhiteSpace(uri) ? null : ResourceManager.GetImageSource(from.ToString());
            return result is not null;
        }
        else
        {
            result = string.Empty;
            return false;
        }
    }
}
