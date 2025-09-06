using System.Data;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Npgsql;

namespace OMNE.Postgres;

[SuppressMessage("Reliability", "CA2000", Justification = "Disposed by base class")]
public class NpgsqlDapperRepository<TEntity>(SqlContext context, ISqlGenerator<TEntity> sqlGenerator)
    : DapperRepository<TEntity>(context.CloneConnection(), sqlGenerator)
    where TEntity : class;

[SuppressMessage("Reliability", "CA2000", Justification = "Disposed by base class")]
public class NpgsqlReadOnlyDapperRepository<TEntity>(SqlContext context, ISqlGenerator<TEntity> sqlGenerator)
    : ReadOnlyDapperRepository<TEntity>(context.CloneConnection(), sqlGenerator)
    where TEntity : class;

file static class Utils
{
    public static IDbConnection CloneConnection(this DbContext context)
    {
        // BUG: ReadOnlyDapperRepository disposes connection which cannot be later be reused by connection pool
        var connection = (NpgsqlConnection)context.Database.GetDbConnection();
        return (IDbConnection)((ICloneable)connection).Clone();
    }
}
