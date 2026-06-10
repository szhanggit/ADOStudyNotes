using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TryCoreBuckInsertWebApp.CommonHelper;
using TryCoreBuckInsertWebApp.Models;

namespace TryCoreBuckInsertWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProvider _dataProvider;

        public HomeController(ILogger<HomeController> logger, IDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        public IActionResult Index()
        {
            List<Destination> list = new List<Destination> {
                new Destination{ Country="China", Name="The Bund0", Description = "The Bund00" },
                new Destination{ Country="China", Name="The Bund1", Description = "The Bund11" },
                new Destination{ Country="China", Name="The Bund2", Description = "The Bund22" },
                new Destination{ Country="China", Name="The Bund3", Description = "The Bund33" },
                new Destination{ Country="China", Name="The Bund4", Description = "The Bund44" },
                new Destination{ Country="China", Name="The Bund5", Description = "The Bund55" },
                new Destination{ Country="China", Name="The Bund6", Description = "The Bund66" },
                new Destination{ Country="China", Name="The Bund7", Description = "The Bund77" },
                new Destination{ Country="China", Name="The Bund8", Description = "The Bund88" },
                new Destination{ Country="China", Name="The Bund9", Description = "The Bund99" },
            };

            DataTable destinations = StaticMemberHelper.DestinationTable();
            DataRow dr = null;

            foreach (Destination des in list)
            {
                dr = destinations.NewRow();
                dr["Name"] = des.Name;
                dr["Country"] = des.Country;
                dr["Description"] = des.Description;
                destinations.Rows.Add(dr);
            }

            _dataProvider.BulkCopyDataTable(destinations);

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
