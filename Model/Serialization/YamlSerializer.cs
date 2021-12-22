using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WwtbamOld.Model;

public sealed class YamlSerializer : Serializer
{
    #region Fields

    public static readonly YamlSerializer Instance;
    private static readonly IDeserializer _deserializer;

    #endregion Fields

    static YamlSerializer()
    {
        Instance = new();
        _deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance)
                                                 .Build();
    }

    private YamlSerializer()
    {
    }

    #region Properties

    public override string Extension => "yaml";

    #endregion Properties

    #region Methods

    public override T Deserialize<T>(string yaml)
    {
        try
        {
            return _deserializer.Deserialize<T>(yaml);
        }
        catch (Exception)
        {
            return default;
        }
    }

    #endregion Methods
}