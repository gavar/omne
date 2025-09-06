using OMNE.Sdk.Services;
using OMNE.Sdk.Transport;

namespace OMNE.Sdk;

[SuppressMessage("ReSharper", "CS9264", Justification = "Lazy Field")]
[SuppressMessage("ReSharper", "NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract", Justification = "Lazy Field")]
public partial class OmneClient : IDisposable
{
    private readonly RestClient client;

    public OmneClient(Uri baseUrl) => client = CreateClient(new(baseUrl));
    public OmneClient(string baseUrl) => client = CreateClient(new(baseUrl));

    /// <summary> Products API. </summary>
    public ProductClient Products => field ??= new(client);

    /// <inheritdoc />
    public void Dispose() => client.Dispose();
}

public partial class OmneClient
{
    private static RestClient CreateClient(RestClientOptions options) => new(
        options,
        configureDefaultHeaders: static headers => headers.Add("Accept", "application/json"),
        configureSerialization: static config => config.UseSerializer(static () => RestJsonSerializer.Default)
    );
}
