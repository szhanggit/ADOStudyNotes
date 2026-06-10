using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Test.Drivers;
using SpecFlow.Test.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlow.Test.Hooks
{
    [Binding]
    public class DriversHook
    {
        private readonly SeleniumDriver seleniumDriver;
        private static IConfiguration _configuration;
        private static IServiceProvider ServiceProvider;

        public DriversHook(SeleniumDriver seleniumDriver)
        {
            this.seleniumDriver = seleniumDriver;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            options.AddArguments("--disable-gpu");
            options.AddArguments("--headless");

            seleniumDriver.WebDriver = new ChromeDriver(options);
            seleniumDriver.WebDriverWait = new WebDriverWait(seleniumDriver.WebDriver, new TimeSpan(0, 10, 0));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            seleniumDriver.WebDriver.Quit();
        }

        [BeforeTestRun]
        public static void InitializeConfig()
        {
            LaunchSettingsFixture();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environmentName}.json", true);

            _configuration = config.Build();
        }

        [Before]
        public void SetUpData()
        {
            Data data = new Data();
            DataManager.SetData(data);
        }

        private static void LaunchSettingsFixture()
        {
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }
    }
}
