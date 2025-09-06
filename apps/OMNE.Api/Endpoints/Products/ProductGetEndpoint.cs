namespace OMNE.Api.Endpoints.Products;

[AllowAnonymous]
[HttpGet("/products/{id}")]
public class ProductGetEndpoint(IReadOnlyDapperRepository<ProductEntity> repository) : EndpointWithoutRequest<Product>
{
    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id", isRequired: true);
        var entity = await repository.FindByIdAsync(id, ct);

        if (entity is null) await Send.NotFoundAsync(ct);
        else await Send.OkAsync(entity.ToProduct(), ct);
    }
}
