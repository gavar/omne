using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp.Serializers;

namespace OMNE.Sdk.Transport;

internal class RestJsonSerializer : IRestSerializer, ISerializer, IDeserializer
{
    public static readonly RestJsonSerializer Default = new();

    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <inheritdoc />
    public ISerializer Serializer => this;

    /// <inheritdoc />
    public IDeserializer Deserializer => this;

    /// <inheritdoc />
    public ContentType ContentType { get; set; } = ContentType.Json;

    /// <inheritdoc />
    [SuppressMessage("Performance", "CA1819", Justification = "IRestSerializer")]
    public string[] AcceptedContentTypes { get; } = [ContentType.Json];

    /// <inheritdoc />
    public SupportsContentType SupportsContentType { get; } = type => type.Value switch
    {
        "application/json" => true,
        _ => false,
    };

    /// <inheritdoc />
    public DataFormat DataFormat => DataFormat.Json;

    /// <inheritdoc />
    public string? Serialize(Parameter parameter)
        => Serialize(parameter.Value);

    /// <inheritdoc />
    public string? Serialize(object? obj) => obj switch
    {
        null => null,
        Payload payload => JsonSerializer.Serialize(payload.Value, payload.Type, Options),
        _ => JsonSerializer.Serialize(obj, Options),
    };

    /// <inheritdoc />
    public T? Deserialize<T>(RestResponse response)
        => response.Content is not null
            ? JsonSerializer.Deserialize<T>(response.Content, Options)
            : default;
}
