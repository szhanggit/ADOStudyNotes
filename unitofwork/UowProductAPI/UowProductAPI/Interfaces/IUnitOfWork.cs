namespace UowProductAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
    }
}
