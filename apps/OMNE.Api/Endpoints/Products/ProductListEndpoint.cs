namespace OMNE.Api.Endpoints.Products;

[AllowAnonymous]
[HttpGet("/products")]
public class ProductListEndpoint(IReadOnlyDapperRepository<ProductEntity> repository) : EndpointWithoutRequest<List<Product>>
{
    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken ct)
    {
        var list = await repository
            .SetOrderBy(OrderInfo.SortDirection.ASC, x => x.Id)
            .FindAllAsync(ct);

        await Send.OkAsync(list.ToProductList(), ct);
    }
}
