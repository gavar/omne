using System.Data;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace OMNE.Data.Model;

public class ProductRepository(IDbConnection connection, ISqlGenerator<ProductEntity> sqlGenerator)
    : DapperRepository<ProductEntity>(connection, sqlGenerator);
