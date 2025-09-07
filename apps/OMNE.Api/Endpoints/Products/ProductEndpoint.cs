namespace OMNE.Api.Endpoints.Products;

[AllowAnonymous]
[HttpPost("/products")]
public class ProductEndpoint(IDapperRepository<ProductEntity> repository) : Endpoint<ProductProps, Product>
{
    /// <inheritdoc />
    public override async Task HandleAsync(ProductProps req, CancellationToken ct)
    {
        var entity = req.ToProductEntity();
        await repository.InsertAsync(entity, ct);

        var dto = entity.ToProduct();
        await Send.CreatedAtAsync<ProductGetEndpoint>(new { id = dto.Id }, dto, cancellation: ct);
    }
}
