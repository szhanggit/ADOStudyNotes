using Core;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Z.Dapper.Plus;

namespace Repository.DapperPlus
{
    public interface IClientRepository : IRepository2<Client>
    {
        IEnumerable<Client> GetClientLazily();
        Task<Client> GetClientByIdAsync(int ClientId, IDbTransaction trans);
        Task<int> GetNewSequenceIdAsync();
    }
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(Context context) : base(context)
        {

        }

        public IEnumerable<Client> GetClientLazily()
        {
            IEnumerable<Client> _clientList = _context.Connection.GetAll<Client>().Where(_ => !string.IsNullOrEmpty(_.Identity_Code));
            return _clientList;
        }

        public async Task<Client> GetClientByIdAsync(int ClientId, IDbTransaction trans)
		{
            IEnumerable<Client> _clientList = _context.Connection.GetAll<Client>(trans).Where(_ => _.Identity_Code.Contains("0000"));
            Client _client = await _context.Connection.GetAsync<Client>(ClientId, trans);           
            return _client;
		}

        public async Task<int> GetNewSequenceIdAsync()
        {
            int NewId = 0;
            Sequence sequenceEF = new Sequence();
            DapperPlusActionSet<Sequence> _obj = null;

            await Task.Factory.StartNew(() =>
            {
                _context.Entity<Sequence>().Table("general.tb_s_sequence").Identity(_ => _.Id);
                _obj = _context.BulkInsert(sequenceEF);
                NewId = _obj?.CurrentItem?.Id ?? 0;
            });
            
            return NewId;
        }


    }
}
