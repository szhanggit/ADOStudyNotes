using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionScopeEx0
{
    public class AdventureWorkRepository
    {
        public void UpdateAddress(int AddressId, string AddressLine2)
        {
            using (var context = new AdventureWorkDBContext())
            {
                Address address = context.Addresses.FirstOrDefault(item => item.AddressID == AddressId);
                if (address != null)
                {
                    address.AddressLine2 = AddressLine2;
                    context.SaveChanges();
                }
            }
        }
    }
}
