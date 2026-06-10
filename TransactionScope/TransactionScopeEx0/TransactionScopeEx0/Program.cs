using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TransactionScopeEx0
{
    class Program
    {
        static void Main(string[] args)
        {

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    AdventureWorkRepository awr = new AdventureWorkRepository();
                    awr.UpdateAddress(1, "asdf");

                    DataProvider.AddContactType("Senior Sales Representative");

                    File.WriteAllLines(@"D:\Study\ADOStudyNotes\TransactionScope\TransactionScopeEx0\File.txt", new string[] { "This is output." });

                    //throw new Exception();

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (TransactionException ex)
                {
                    Console.WriteLine(ex.Message);
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transactionScope.Dispose();
                }
            }
        }
    }
}
