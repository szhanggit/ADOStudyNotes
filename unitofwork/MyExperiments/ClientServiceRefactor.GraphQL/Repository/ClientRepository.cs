using Core;
using Dapper.Contrib.Extensions;
using Domain.Entities;
using System.Data;

namespace Repository
{
    public interface IClientRepository : IRepository2<Client>
    {
        Task<int> GetNewSequenceIdAsync();
    }
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(Context context) : base(context)
        {

        }

        public async Task<int> GetNewSequenceIdAsync()
        {
            int NewId = 0;
            Sequence sequence = new Sequence { Val = 1};

            NewId = await _context.Connection.InsertAsync(sequence);

            return NewId;
        }
    }
}
