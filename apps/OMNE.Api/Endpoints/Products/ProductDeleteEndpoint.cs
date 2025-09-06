namespace OMNE.Api.Endpoints.Products;

[AllowAnonymous]
[HttpDelete("/products/{id}")]
public class ProductDeleteEndpoint(IDapperRepository<ProductEntity> repository) : EndpointWithoutRequest
{
    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id", isRequired: true);
        var success = await repository.DeleteAsync(x => x.Id == id, ct);

        if (success) await Send.NoContentAsync(ct);
        else await Send.NotFoundAsync(ct);
    }
}
