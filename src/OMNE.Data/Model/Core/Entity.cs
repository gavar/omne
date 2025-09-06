using MicroOrm.Dapper.Repositories.Attributes;

namespace OMNE.Data.Model;

public class Entity
{
    [Key]
    [Identity]
    [Column(TypeName = "bigint")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }
}
