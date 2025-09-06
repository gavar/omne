namespace OMNE.Model;

/// <summary> Base class for RESTful resources. </summary>
public class Resource : IResource
{
    /// <inheritdoc />
    public ulong Id { get; set; }
}

/// <summary> Represents a RESTful resource. </summary>
public interface IResource
{
    /// <summary> Unique identifier of the resource. </summary>
    ulong Id { get; }
}
