using Grpc.Core;
using Service.Protos;
using static RepositoryDapper.UnitOfWork.SalesUnit;
using e= Entities;

namespace Service.Services
{
    public class ProductService : Products.ProductsBase
    {
        ISalesUnitOfWork _unit;
        public ProductService(ISalesUnitOfWork unit, IConfiguration config)
        {
            unit.SetConnection(config["ConnectionStrings:local"]);
            _unit = unit;
        }
        public override async Task<UpsertProductResponse> Create(UpsertProductRequest request, ServerCallContext context)
        {
            try
            {
                var entity = new e.Product
                {
                    Price = Convert.ToDecimal(request.Entity.Price.Units + (request.Entity.Price.Nanos * 0.01)),
                    Description = request.Entity.Description,
                    Unit = request.Entity.Unit
                };
                var res = await _unit.ProductRepo.Add(entity);
                return new UpsertProductResponse { Id = res };
            }
            catch(Exception ex)
            {
                return new UpsertProductResponse { Id = 0 };
            }
            
        }

        public override async Task<RemoveProductResponse> Remove(RemoveProductRequest request, ServerCallContext context)
        {
            try
            {
                var res = await _unit.ProductRepo.DeleteProduct(request.Id);
                if (res)
                    return new RemoveProductResponse { Deleted = true };
                else
                    return new RemoveProductResponse { Deleted = false };
            }
            catch (Exception ex)
            {
                return new RemoveProductResponse { Deleted = false };
            }
        }

        public override async Task<UpsertProductResponse> Update(UpsertProductRequest request, ServerCallContext context)
        {
            try
            {
                var entity = new e.Product
                {
                    Product_Id = request.Entity.Id,
                    Price = Convert.ToDecimal(request.Entity.Price.Units + (request.Entity.Price.Nanos * 0.01)),
                    Unit = request.Entity.Unit
                };
                var res = await _unit.ProductRepo.Update(entity);
                if (res)
                    return new UpsertProductResponse { Id = entity.Product_Id };
                else
                    return new UpsertProductResponse { Id = 0 };
            }
            catch(Exception ex)
            {
                return new UpsertProductResponse { Id = 0 };
            }
        }
    }
}
