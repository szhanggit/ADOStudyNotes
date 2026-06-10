using Microsoft.Extensions.Configuration;
using ReadScriptFromFile.Data;
using System;
using System.IO;

namespace ReadScriptFromFile
{
    class Program
    {
        private static IConfiguration _configuration;
        private static Repository _repo;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", true);

            _configuration = config.Build();

            string connString = _configuration.GetConnectionString("TxProgram");
            _repo = new Repository(connString);

            _repo.Execute(new Model
            {
                ScriptFile = Path.Combine("TxProgram", "CleanUpPrograms.sql")
            });
        }
    }
}
