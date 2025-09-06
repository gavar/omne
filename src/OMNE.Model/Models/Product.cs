using System.ComponentModel.DataAnnotations;
using Riok.Mapperly.Abstractions;

namespace OMNE.Model;

[Mapper(UseReferenceHandling = false, IncludedMembers = MemberVisibility.Public)]
public partial class Product : ProductProps, IResource
{
    /// <inheritdoc />
    public ulong Id { get; set; }

    /// <summary> Map properties from source to target. </summary>
    public static partial void Map(Product source, Product target);
}

/// <summary> Mutable properties of a <see cref="Product" />. </summary>
public class ProductProps
{
    /// <summary> Name of the product. </summary>
    /// <value>A non-empty string representing the product name.</value>
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = default!;

    /// <summary> Price of the product. </summary>
    /// <value>A decimal value up to 12 total digits and 3 decimal places for precision.</value>
    /// <remarks>
    /// The price is stored with high precision (12,3) allowing for values up to 999,999,999.999.
    /// The database storage typically requires 6 bytes for values with 9-12 digits of precision.
    /// </remarks>
    public decimal Price { get; set; }

    /// <summary> Optional description of the product. </summary>
    public string? Description { get; set; }
}
