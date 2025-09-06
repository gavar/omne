namespace OMNE.Sdk.Transport;

internal class Request(IRestClient client) : RestRequest, IRequest<RestResponse>
{
    private readonly IRestClient client = client;

    /// <summary> A token to observe while waiting for the request to complete. </summary>
    public CancellationToken Cancellation { get; init; }

    /// <inheritdoc />
    public async Task<RestResponse> Send(CancellationToken cancellation = default)
    {
        // execute
        var raw = await client
            .ExecuteAsync(this, CreateLinkedTokenSource(Cancellation, cancellation))
            .ConfigureAwait(false);

        // interceptors
        if (Interceptors is { } interceptors)
            foreach (var interceptor in interceptors)
                await interceptor.BeforeDeserialization(raw, cancellation).ConfigureAwait(false);

        // done
        return raw.ThrowIfError();
    }

    protected static Task<T> Send<T>(Request request, CancellationToken cancellation = default)
    {
        return request
            .Send(cancellation)
            .ContinueWith(DeserializeRest!, request, cancellation, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Current);

        static T DeserializeRest(Task<RestResponse> task, object state)
            => ((Request)state).client.Serializers.DeserializeContent<T>(task.Result)!;
    }

    private static CancellationToken CreateLinkedTokenSource(CancellationToken a, CancellationToken b)
    {
        if (a.Equals(b)) return a;
        if (!a.CanBeCanceled) return b;
        if (!b.CanBeCanceled) return a;
        return CancellationTokenSource.CreateLinkedTokenSource(a, b).Token;
    }
}

internal class Request<T>(IRestClient client) : Request(client), IRequest<T>
{
    /// <inheritdoc />
    public new Task<T> Send(CancellationToken cancellation = default)
        => Send<T>(this, cancellation);
}

internal class Request<TForm, TReply>(IRestClient client) : Request(client), IRequest<TReply>
{
    /// <inheritdoc cref="RestRequestExtensions.AddBody" />
    public object Body { init => this.AddJsonBody(value); }

    /// <inheritdoc />
    public new Task<TReply> Send(CancellationToken cancellation = default)
        => Send<TReply>(this, cancellation);
}
