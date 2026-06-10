using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test.Drivers
{
    public class SeleniumDriver
    {
        public IWebDriver WebDriver { get; set; }
        public WebDriverWait WebDriverWait { get; set; }
    }
}
