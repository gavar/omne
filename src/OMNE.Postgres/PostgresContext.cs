namespace OMNE.Postgres;

public class PostgresContext(DbContextOptions<PostgresContext> options) : SqlContext(options);
