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
    public sealed class CreateNewTenantPage
    {
        private WebDriverWait WebDriverWait { get; }
        public CreateNewTenantPage(SeleniumDriver seleniumDriver)
        {
            WebDriverWait = seleniumDriver.WebDriverWait;
        }

        public IWebElement Name => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("txtName"));
        });

        public IWebElement CountryCode => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//ng-select[@id='cboCountries']//input[@type='text']"));
        });


        public IWebElement SpanCountryCode => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//span[@ng-reflect-ng-item-label='Philippines']"));
        });

        public IWebElement TimeZone => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//ng-select[@id='cboTimeZone']//input[@type='text']"));
        });

        public IWebElement SpanTimeZone => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//span[@ng-reflect-ng-item-label='Philippines UTC+08:00']"));
        });

        public IWebElement TimeFormat => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//ng-select[@id='cboTimeFormat']//input[@type='text']"));
        });

        public IWebElement SpanTimeFormat => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//span[@ng-reflect-ng-item-label='yyyy/MM/dd']"));
        });

        public IWebElement CurrencySymbol => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("txtCurrencySymbol"));
        });


        public IWebElement RbtBefore => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("rbtBefore"));
        });

        public IWebElement RbtAfter => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("rbtAfter"));
        });

        public IWebElement CompanyTaxRate => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("txtCompanyTaxRate"));
        });

        public IWebElement EffectivityDate => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("dtEffectivityDate"));
        });

        public IWebElement Language => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//ng-select[@id='cboLanguage']//input[@type='text']"));
        });

        public IWebElement SpanLanguage => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//span[@ng-reflect-ng-item-label='Tagalog']"));
        });
        public IWebElement SaveButton => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("btnSave"));
        });

        public IWebElement FileUpload => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.Id("imgLogo"));
        });

        public IWebElement SuccessMessage => WebDriverWait.Until(w =>
        {
            return w.FindElement(By.XPath("//div[@id='toast-container']//div//div[@role='alertdialog']"));
        });

        public void ClickSaveBtn() => SaveButton.Click();
    }
}
