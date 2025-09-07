using System.Net.Http.Json;

namespace OMNE.Api.Fixtures;

public static class TestUtils
{
    public static async Task<T> Json<T>(this HttpResponseMessage response)
        => (await response.Content.ReadFromJsonAsync<T>())!;

    public static async Task<T> Json<T>(this Task<HttpResponseMessage> task)
        => await (await task).Json<T>();
}
