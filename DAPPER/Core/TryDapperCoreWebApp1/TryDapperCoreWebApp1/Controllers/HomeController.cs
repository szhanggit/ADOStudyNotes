using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TryDapperCoreWebApp1.Models;
using TryDapperCoreWebApp1.Services;

namespace TryDapperCoreWebApp1.Controllers
{
    /*
     https://www.c-sharpcorner.com/article/using-dapper-in-asp-net-core-web-api/
     */
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDapper _dapper;

        public HomeController(ILogger<HomeController> logger, IDapper dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<IActionResult> Index()
        {
            int Id = 28;
            var result = await Task.FromResult(_dapper.Get<Logs>($"select * from [dbo].[logs] with(nolock) where Id = {Id}", null, commandType: CommandType.Text));

            string token = "Something";
            var totalcount = Task.FromResult(_dapper.Get<int>($"select COUNT(*) from [logs] with(nolock) WHERE message like '%{token}%'", null, commandType: CommandType.Text));

            //var dbparams = new DynamicParameters();
            //dbparams.Add("Message", "Hello World!", DbType.String);
            //dbparams.Add("Level", "Info", DbType.String);
            //var result2 = await Task.FromResult(_dapper.Insert<int>("[dbo].[spAddLogs]", dbparams, commandType: CommandType.StoredProcedure));

            //var result3 = await Task.FromResult(_dapper.Execute($"Delete [dbo].[spAddLogs] Where Id = {Id}", null, commandType: CommandType.Text));

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
