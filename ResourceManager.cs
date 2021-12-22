using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WwtbamOld;

internal static class ResourceManager
{
    #region Графика

    private const string GRAPHICS_PREFIX = @"Resources\Graphics";

    public static string GetFileImagePath(string folders, string fileName, string extension)
        => Path.GetFullPath(Path.Combine(GRAPHICS_PREFIX, folders, $"wwtbam {fileName}.{extension}"));

    public static ImageSource GetFileImageSource(string folders, string fileName, string extension)
        => GetFileImageSource(GetFileImagePath(folders, fileName, extension));

    public static ImageSource GetFileImageSource(string filePath)
        => GetFileImageSource(new Uri(filePath, UriKind.Absolute));

    private static ImageSource GetFileImageSource(Uri uri)
        => (ImageSource)new ImageSourceConverter().ConvertFrom(uri);

    #endregion Графика

    #region Ресурсы

    public static string GetResourceName(string fileName, ResourceType resourceType, string extension, params string[] folders)
    {
        StringBuilder stringBuilder = new();
        char sep = '.';

        stringBuilder.Append("WwtbamOld");
        stringBuilder.Append(sep);

        if (resourceType == ResourceType.None)
        {
            if (folders.Length > 0)
                stringBuilder.Append(string.Join('.', folders).Replace(' ', '_')).Append(sep);

            stringBuilder.AppendJoin(sep, $"{fileName}.{extension}");
        }
        else
        {
            stringBuilder.AppendJoin(sep, "Resources", resourceType.ToString());

            if (folders.Length > 0)
                stringBuilder.Append(sep).Append(string.Join('.', folders).Replace(' ', '_'));

            stringBuilder.Append(sep).AppendJoin(sep, $"{(resourceType != ResourceType.Fonts ? "wwtbam " : "")}{fileName}.{extension}");
        }

        return stringBuilder.ToString();
    }

    private static Stream GetStreamByResourceName(string resourceName)
        => Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

    public static string GetFontName(string fileName, string extension, params string[] folders)
        => GetResourceName(fileName, ResourceType.Fonts, extension, folders);

    public static string GetResourceFileContents(string fileName, string extension, params string[] folders)
        => GetResourceFileContents(GetResourceName(fileName, ResourceType.None, extension, folders));

    public static string GetResourceFileContents(string resourceName)
        => GetResourceFileContents(GetStreamByResourceName(resourceName));

    public static string GetResourceFileContents(Stream stream)
    {
        using StreamReader streamReader = new(stream);
        return streamReader.ReadToEnd();
    }

    public static string GetResourceGraphicsName(string fileName, string extension, params string[] folders)
        => GetResourceName(fileName, ResourceType.Graphics, extension, folders);

    public static ImageSource GetImageSource(string fileName, string extension, params string[] folders)
        => GetImageSource(GetResourceGraphicsName(fileName, extension, folders));

    public static ImageSource GetImageSource(string resourceName)
    {
        BitmapImage image = new();
        image.BeginInit();
        image.StreamSource = GetStreamByResourceName(resourceName);
        image.EndInit();

        return image;
    }

    public static string GetResourceAudioName(string fileName, string extension = "mp3", params string[] folders)
        => GetResourceName(fileName, ResourceType.Audio, extension, folders);

    public static Stream GetAudioStream(string fileName, string extension = "mp3", params string[] folders)
        => GetAudioStream(GetResourceAudioName(fileName, extension, folders));

    public static Stream GetAudioStream(string resourceName)
        => GetStreamByResourceName(resourceName);

    #endregion Ресурсы
}