using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TryADOWebApp1.Models;

namespace TryADOWebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            string connectionString = _config.GetValue<string>("ConnectionString");
            SqlConnection cn = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter("select student_classid, studentid, classid from student_class", cn);
            DataSet ds = new DataSet();
            da.Fill(ds, "table1");
            DataTable Dt = ds.Tables["table1"];
            DataTableReader Dtr = Dt.CreateDataReader();

            while (Dtr.Read())
            {
                String student_classid = Dtr["student_classid"].ToString();
                String studentid = Dtr["studentid"].ToString();
                String classid = Dtr["classid"].ToString();
            }

            Dtr.Close();
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
