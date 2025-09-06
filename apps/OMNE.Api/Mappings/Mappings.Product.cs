namespace OMNE.Api.Mappings;

internal static partial class Mappings
{
    public static partial Product ToProduct(this ProductEntity source);
    public static partial ProductEntity ToProductEntity(this ProductProps source);
    public static partial List<Product> ToProductList(this IEnumerable<ProductEntity> source);
    public static partial void MapProductEntity(this ProductProps source, ProductEntity target);
}
