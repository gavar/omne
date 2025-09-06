namespace OMNE.Data.Model;

[Table("product")]
public class ProductEntity : Entity
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
    [Precision(12, 3)]
    public decimal Price { get; set; }

    /// <summary> Optional description of the product. </summary>
    public string? Description { get; set; }
}
