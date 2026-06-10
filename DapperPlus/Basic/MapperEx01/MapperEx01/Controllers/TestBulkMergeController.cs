using MapperEx01.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Z.Dapper.Plus;

namespace MapperEx01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestBulkMergeController : ControllerBase
    {
        private readonly IConfiguration configuration;
		private int Counter;

		public TestBulkMergeController(IConfiguration config)
        {
			configuration = config;
		}

        [HttpPost(Name = "TestUpdate")]
        public void UpdateCustomer()
        {
			string connStr = configuration.GetSection("ConnectionStrings").GetSection("connectionString").Value;
			GenerateDatabase(connStr);

			DapperPlusManager.Entity<Customer>().Table("Customers").Identity(x => x.CustomerID);


			var connection = new SqlConnection(connStr);
			var customers = GenerateCustomers(5);

			connection.BulkInsert(customers);
			// UPDATE 3 customers
			customers.Take(3).ToList().ForEach(x => x.CustomerName += "_Updated");
			// ADD 3 new customers
			customers.AddRange(GenerateCustomers(3));

			connection.BulkMerge(customers);
		}

		private List<Customer> GenerateCustomers(int count)
		{
			count += Counter;

			var customers = new List<Customer>();

			for (; Counter < count; Counter++)
			{
				customers.Add(new Customer() { CustomerName = "CustomerName_" + Counter, ContactName = "ContactName_" + Counter, Address = "Address_" + Counter, City = "City_" + Counter, PostalCode = "PostalCode_" + Counter, Country = "Country_" + Counter });
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
