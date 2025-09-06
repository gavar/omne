using OMNE.Model;
using OMNE.Sdk.Transport;

namespace OMNE.Sdk.Services;

public class ProductClient(IRestClient client)
{
    public IRequest<List<Product>> List(CancellationToken cancellation = default) => new Request<List<Product>>(client)
    {
        Resource = "/products",
        Cancellation = cancellation,
    };

    public IRequest<Product> Get(ulong id, CancellationToken cancellation = default) => new Request<Product>(client)
    {
        Resource = $"/products/{id}",
        Cancellation = cancellation,
    };

    public IRequest<Product> Create(ProductProps form, CancellationToken cancellation = default) => new Request<ProductProps, Product>(client)
    {
        Resource = "/products",
        Cancellation = cancellation,
        Body = form,
    };

    public async Task Update(Product product, CancellationToken cancellation = default)
        => Product.Map(await Update(product.Id, product, cancellation), product);

    public IRequest<Product> Update(ulong id, ProductProps form, CancellationToken cancellation = default) => new Request<ProductProps, Product>(client)
    {
        Resource = $"/products/{id}",
        Cancellation = cancellation,
        Body = form,
    };

    public Task Delete(ulong id, CancellationToken cancellation = default) => new Request(client)
    {
        Resource = $"/products/{id}",
    }.Send(cancellation);
}
