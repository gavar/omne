namespace OMNE.Data;

public class SqlContext : DbContext
{
    public SqlContext() { }
    public SqlContext(DbContextOptions options) : base(options) { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        var assemblies = new[] { GetType(), typeof(SqlContext) }
            .Select(x => x.Assembly)
            .Distinct();

        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}
