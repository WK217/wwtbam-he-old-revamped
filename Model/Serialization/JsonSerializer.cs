using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WwtbamOld.Model;

public sealed class JsonSerializer : Serializer
{
    #region Fields

    public static readonly JsonSerializer Instance;
    private static readonly JsonSerializerOptions _jsonSerializerOptions;

    #endregion Fields

    static JsonSerializer()
    {
        Instance = new();
        _jsonSerializerOptions = new()
        {
            Converters = { new JsonStringEnumConverter() },
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyProperties = true
        };
    }

    private JsonSerializer()
    {
    }

    #region Properties

    public override string Extension => "json";

    #endregion Properties

    #region Methods

    public override T Deserialize<T>(string json)
    {
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
        catch (Exception)
        {
            return default;
        }
    }

    #endregion Methods
}