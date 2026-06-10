using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Test.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test.Pages.Tenant
{
    public sealed class TenantMainPage
    {
        private WebDriverWait WebDriverWait { get; }
        public TenantMainPage(SeleniumDriver seleniumDriver)
        {
            WebDriverWait = seleniumDriver.WebDriverWait;
        }

        public IWebElement CreateButton => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("btnCreate"));
        });
    }
}
