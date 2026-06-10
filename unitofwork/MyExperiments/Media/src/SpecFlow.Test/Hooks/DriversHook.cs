using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Test.Drivers;
using System;
using System.Collections.Generic;
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
    }
}
