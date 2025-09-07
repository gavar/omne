namespace OMNE.Sdk.Transport;

public sealed record Payload(object Value, Type Type)
{
    public static Payload Create<T>(T value) where T : class => new(value, typeof(T));
}
