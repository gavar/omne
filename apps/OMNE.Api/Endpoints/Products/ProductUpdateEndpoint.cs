namespace OMNE.Api.Endpoints.Products;

[AllowAnonymous]
[HttpPut("/products/{id}")]
public class ProductUpdateEndpoint(IDapperRepository<ProductEntity> repository) : Endpoint<ProductProps, Product>
{
    /// <inheritdoc />
    public override async Task HandleAsync(ProductProps req, CancellationToken ct)
    {
        var id = Route<long>("id", isRequired: true);
        if (await repository.FindByIdAsync(id, ct) is { } entity)
        {
            req.MapProductEntity(entity);
            await repository.UpdateAsync(entity, ct);
            await Send.OkAsync(entity.ToProduct(), cancellation: ct);
        }
        else
        {
            await Send.NotFoundAsync(ct);
        }
    }
}
