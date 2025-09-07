using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace OMNE.Api.Fixtures;

[SuppressMessage("Design", "CA1054", Justification = "Unnecessary")]
[SuppressMessage("Reliability", "CA2000", Justification = "Unnecessary")]
[SuppressMessage("Usage", "CA2234", Justification = "By Design")]
public class IntegrationTest(IntegrationTestFixture fixture)
{
    protected HttpClient Http => fixture.Http;

    public Task<HttpResponseMessage> GetAsync(string uri)
        => Http.GetAsync(uri, TestContext.Current.CancellationToken);

    public Task<HttpResponseMessage> PutAsync<T>(string uri, T content)
        => Http.PutAsync(uri, JsonContent.Create(content), TestContext.Current.CancellationToken);

    public Task<HttpResponseMessage> PostAsync<T>(string uri, T content)
        => Http.PostAsync(uri, JsonContent.Create(content), TestContext.Current.CancellationToken);

    public Task<HttpResponseMessage> DeleteAsync(string uri)
        => Http.DeleteAsync(uri, TestContext.Current.CancellationToken);

    /// <summary> Truncate DB data. </summary>
    public Task Truncate() => fixture.Truncate();

    /// <summary> Create a snapshot that automatically cleans up data upon disposure. </summary>
    public IAsyncDisposable Snapshot() => new Lease(fixture.Truncate);
}

file sealed class Lease(Func<Task> dispose) : IAsyncDisposable
{
    public ValueTask DisposeAsync() => new(dispose());
}
