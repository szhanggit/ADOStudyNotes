using Dapper;

namespace DalSessionAPI
{
    public class MyRepository
    {
        public MyRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        private IUnitOfWork unitOfWork = null;

        //You also need to handle other parameters like 'sql', 'param' ect. This is out of scope of this answer.
        public Product Get(int id)
        {
            return unitOfWork.Connection.QuerySingleOrDefault<Product>("select * from [product].[tb_p_product] with(nolock) where product_id = @id", new { id }, unitOfWork.Transaction);
        }

        public void Insert(Product poco)
        {
            unitOfWork.Connection.Execute("", null, unitOfWork.Transaction);
        }
    }
}
