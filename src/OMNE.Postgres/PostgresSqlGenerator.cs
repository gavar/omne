using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace OMNE.Postgres;

public class PostgresSqlGenerator<TEntity>() : SqlGenerator<TEntity>(SqlProvider.PostgreSQL) where TEntity : class;
