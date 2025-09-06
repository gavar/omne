namespace OMNE.Web.Configuration;

public static class ServiceDiscovery
{
    private static readonly string[] Protocols = ["https", "http"];

    /// <summary> Gets the endpoint URLs of Aspire resources matching the resource name. </summary>
    /// <param name="config">Configuration to resolve urls from.</param>
    /// <param name="resource">Names of Aspire resources.</param>
    public static IEnumerable<string> GetServiceEndpoints(this IConfiguration config, string resource)
    {
        var root = config.GetSection("Services").GetSection(resource);
        foreach (var protocol in Protocols)
            foreach (var item in root.GetSection(protocol).GetChildren())
                if (item.Value is { Length: > 0 } endpoint)
                    yield return endpoint;
    }
}
