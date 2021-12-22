using System;
using System.Collections.Generic;
using System.IO;

namespace WwtbamOld.Model;

public abstract class Serializer
{
    #region Properties

    public virtual string Extension => null;
    protected string DefaultQuizbaseFilename => Path.ChangeExtension("quizbase", Extension);

    #endregion Properties

    #region Methods

    public abstract T Deserialize<T>(string text);

    public T Read<T>()
    {
        Type type = typeof(T);

        if (type.Equals(typeof(IEnumerable<Quiz>)))
            return (T)Read<IEnumerable<Quiz>>(DefaultQuizbaseFilename);

        return default;
    }

    public T Read<T>(string filename) => Read<T>(new FileInfo(filename));

    public T Read<T>(FileInfo fileInfo)
    {
        try
        {
            return Deserialize<T>(SerializationManager.ReadFileContents(fileInfo));
        }
        catch (Exception)
        {
            return default;
        }
    }

    public IEnumerable<Quiz> ReadQuizbase() => Read<IEnumerable<Quiz>>();

    public IEnumerable<Quiz> ReadQuizbase(string filename) => ReadQuizbase(new FileInfo(filename));

    public IEnumerable<Quiz> ReadQuizbase(FileInfo fileInfo) => Read<IEnumerable<Quiz>>(fileInfo);

    #endregion Methods
}