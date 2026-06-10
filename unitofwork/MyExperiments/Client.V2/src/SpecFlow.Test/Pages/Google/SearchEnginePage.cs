using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Test.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test.Pages.Google
{
    public sealed class SearchEnginePage
    {
        private WebDriverWait WebDriverWait { get; }
        public SearchEnginePage(SeleniumDriver seleniumDriver)
        {
            WebDriverWait = seleniumDriver.WebDriverWait;
        }
        public IWebElement Query => WebDriverWait.Until((w) =>
        {
            return w.FindElement(By.Name("q"));
        });

        public void SearchFor(string value)
        {
            Query.SendKeys(value);
        }

        public void SubmitSearch() => Query.Submit();
    }
}
