using System;
using System.Collections.Generic;
using System.IO;

namespace WwtbamOld.Model;

public static class SerializationManager
{
    #region Serializers

    public static XmlSerializer Xml => XmlSerializer.Instance;
    public static JsonSerializer Json => JsonSerializer.Instance;
    public static YamlSerializer Yaml => YamlSerializer.Instance;

    #endregion Serializers

    public static T Read<T>()
    {
        Type type = typeof(T);

        if (type.Equals(typeof(IEnumerable<Quiz>)))
            return (T)Xml.Deserialize<IEnumerable<Quiz>>(ResourceManager.GetResourceFileContents("quizzes", Xml.Extension));

        return default;
    }

    public static T Read<T>(string filename) => Read<T>(new FileInfo(filename));

    public static T Read<T>(FileInfo fileInfo)
    {
        string text = ReadFileContents(fileInfo);

        if (text.IsXml())
            return Xml.Deserialize<T>(text);

        if (text.IsJson())
            return Json.Deserialize<T>(text);

        return Yaml.Deserialize<T>(text);
    }

    public static IEnumerable<Quiz> ReadQuizbase() => Read<IEnumerable<Quiz>>();

    public static IEnumerable<Quiz> ReadQuizbase(string filename) => ReadQuizbase(new FileInfo(filename));

    public static IEnumerable<Quiz> ReadQuizbase(FileInfo fileInfo) => Read<IEnumerable<Quiz>>(fileInfo);

    public static string ReadFileContents(string filename) => ReadFileContents(new FileInfo(filename));

    public static string ReadFileContents(FileInfo fileInfo)
    {
        try
        {
            using FileStream stream = fileInfo.OpenRead();
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool IsXml(this string text)
    {
        string trimmed = text.Trim();
        return trimmed.StartsWith('<') && trimmed.EndsWith('>');
    }

    private static bool IsJson(this string text)
    {
        string trimmed = text.Trim();
        return trimmed.StartsWith('{') && trimmed.EndsWith('}') || trimmed.StartsWith('[') && trimmed.EndsWith(']');
    }
}