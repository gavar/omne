using MicroOrm.Dapper.Repositories.Attributes;

namespace OMNE.Data.Model;

[NotMapped]
public abstract class Entity
{
    [Key]
    [Identity]
    [Column("id", TypeName = "bigint")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}
