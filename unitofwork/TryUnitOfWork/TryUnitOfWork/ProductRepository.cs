using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace TryUnitOfWork
{
    public class ProductRepository : IProductRepository
    {
        protected readonly IDbConnection connection;
        protected readonly IDbTransaction transaction;

        public ProductRepository(UnitOfWork unitOfWork)
        {
            connection = unitOfWork.Transaction.Connection;
            transaction = unitOfWork.Transaction;
        }

        public Product Read(int id)
        {
            return connection.QuerySingleOrDefault<Product>("select * from [product].[tb_p_product] with(nolock) where product_id = @id", new { id }, transaction: transaction);
        }

        public async Task<Product> ReadAsync(int id)
        { 
            return await connection.QuerySingleOrDefaultAsync<Product>("select * from [product].[tb_p_product] with(nolock) where product_id = @id", new { id }, transaction: transaction);
        }
    }
}
