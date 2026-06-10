using MapperEx01.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Z.Dapper.Plus;

namespace MapperEx01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMapperController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public TestMapperController(IConfiguration config)
        {
            configuration = config;
        }

        [HttpGet(Name = "TestMapper")]
        public void GetMap()
        {
            string connStr = configuration.GetSection("ConnectionStrings").GetSection("connectionString").Value;
			GenerateDatabase(connStr);

			// Easy to use
			DapperPlusManager.Entity<Customer>().Table("Customers").Identity(x => x.CustomerID);

			// Easy to customize
			DapperPlusManager.Entity<Customer>("TheMappingKey").Table("Customers")
											 .Identity(x => x.CustomerID)
											 .Map(x => new { x.CustomerName, x.ContactName })
											 .BatchSize(200);

			var connection = new SqlConnection(connStr);
			var listA = GenerateCustomers("CustomerName_A", 5);
			var listB = GenerateCustomers("CustomerName_B", 5);

			// ALL properties will be inserted, and the CustomerID identity value is returned
			var c = connection.BulkInsert(listA);

			// Only CustomerName and ContactName property are inserted, and the CustomerID identity value is returned
			var s = connection.BulkInsert("TheMappingKey", listB);
		}

		private List<Customer> GenerateCustomers(string prefix, int count)
		{
			var customers = new List<Customer>();

			for (int i = 0; i < count; i++)
			{
				customers.Add(new Customer() { CustomerName = prefix + "_" + i, ContactName = "ContactName_" + i, Address = "Address_" + i, City = "City_" + i, PostalCode = "PostalCode_" + i, Country = "Country_" + i });
			}

			return customers;
		}

		private void GenerateDatabase(string connStr)
		{
			using (var connection = new SqlConnection(connStr))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = @"   
						IF (NOT EXISTS (SELECT * 
										 FROM INFORMATION_SCHEMA.TABLES 
										 WHERE TABLE_NAME = 'Customers'))
						begin
							CREATE TABLE [Customers]
							(
								[CustomerID] [INT] IDENTITY(1,1) NOT NULL,
								[CustomerName] [NVARCHAR](255) NULL,
								[ContactName] [NVARCHAR](255) NULL,
								[Address] [NVARCHAR](255) NULL,
								[City] [NVARCHAR](255) NULL,
								[PostalCode] [NVARCHAR](255) NULL,
								[Country] [NVARCHAR](255) NULL,
								CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
								(
									[CustomerID] ASC
								)
							)
						end";
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
