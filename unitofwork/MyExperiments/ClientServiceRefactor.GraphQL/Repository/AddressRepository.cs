using Core;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using System.Data;

namespace Repository
{
    public interface IAddressRepository : IRepository2<Address>
    {
        Task CreateAddress(Address address, IDbTransaction trans);
    }
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(Context context) : base(context)
        {

        }

        public async Task CreateAddress(Address address, IDbTransaction trans)
        {
            var _obj = await _context.Connection.InsertAsync(address, trans);
        }
    }
}
