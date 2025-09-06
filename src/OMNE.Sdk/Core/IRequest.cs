using System.Runtime.CompilerServices;

namespace OMNE.Sdk;

public interface IRequest;

public interface IRequest<TReply> : IRequest
{
    /// <summary> Initiate request execution using current configuration. </summary>
    /// <returns> An asynchronous task with the resource collection response. </returns>
    Task<TReply> Run() => Send();

    /// <summary> Initiate request execution using current configuration. </summary>
    /// <param name="cancellation">A token to observe while waiting for the task to complete.</param>
    /// <returns> An asynchronous task with the resource collection response. </returns>
    Task<TReply> Send(CancellationToken cancellation = default);

    /// <summary> Initiate request execution using current configuration. </summary>
    /// <returns> An object to use for awaiting the collection response. </returns>
    TaskAwaiter<TReply> GetAwaiter() => Send().GetAwaiter();
}
