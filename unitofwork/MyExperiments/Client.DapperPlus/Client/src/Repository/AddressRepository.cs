using Core;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IAddressRepository : IRepository2<Address>
    {
        IEnumerable<Address> GetAddressLazily();
        Task CreateAddress(Address address, IDbTransaction trans);
    }
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(Context context) : base(context)
        {

        }

        public IEnumerable<Address> GetAddressLazily()
        {
            IEnumerable<Address> _addressList = _context.Connection.GetAll<Address>().Where(_ => _.Address_Id > 0);
            return _addressList;
        }

        public async Task CreateAddress(Address address, IDbTransaction trans)
        {
            _context.Entity<Address>().Table("general.tb_a_address").IgnoreOnMergeUpdate(_ => new { _.TimeStamp }).Identity(_ => _.Address_Id);
            var _obj = await _context.Connection.InsertAsync(address, trans);
        }
    }
}
