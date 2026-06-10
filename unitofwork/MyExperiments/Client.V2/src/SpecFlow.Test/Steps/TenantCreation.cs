using SpecFlow.Test.Drivers;
using SpecFlow.Test.Pages.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public sealed class TenantCreation
    {
        private readonly TenantMainPage tenantMainPage;
        private readonly CreateNewTenantPage createNewTenantPage;
        private readonly SeleniumDriver seleniumDriver;
        public TenantCreation(SeleniumDriver seleniumDriver)
        {
            this.seleniumDriver = seleniumDriver;
            tenantMainPage = new TenantMainPage(seleniumDriver);
            createNewTenantPage = new CreateNewTenantPage(seleniumDriver);
        }
        [Given(@"I launch the tenant application")]
        public void GivenILaunchTheTenantApplication()
        {
            seleniumDriver.WebDriver.Navigate().GoToUrl("http://localhost:4200");
        }

        [Given(@"I navigate to tenant")]
        public void GivenINavigateToTenant()
        {
            seleniumDriver.WebDriver.Navigate().GoToUrl("http://localhost:4200/module/tenant");
        }

        [Given(@"I click the button create")]
        public void GivenIClickTheButtonCreate()
        {
            tenantMainPage.CreateButton.Click();
        }

        [Given(@"I enter the tenant information")]
        public void GivenIEnterTheTenantInformation(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            var serialized = JsonSerializer.Serialize(data);
            TenantDetail detail = JsonSerializer.Deserialize<TenantDetail>(serialized);
            var effectivityDate = (Convert.ToDateTime(detail.EffectivityDate).ToString("MM-dd-yyyy")).Split("-").ToList();
            createNewTenantPage.Name.SendKeys(detail.TenantName);
            createNewTenantPage.CountryCode.SendKeys(detail.Country);
            createNewTenantPage.SpanCountryCode.Click();
            createNewTenantPage.TimeZone.SendKeys(detail.TimeZone);
            createNewTenantPage.SpanTimeZone.Click();
            createNewTenantPage.TimeFormat.SendKeys(detail.TimeFormat);
            createNewTenantPage.SpanTimeFormat.Click();
            createNewTenantPage.CompanyTaxRate.SendKeys(detail.CompanyTaxRate.ToString());
            createNewTenantPage.EffectivityDate.Click();
            effectivityDate.ForEach(fe =>  createNewTenantPage.EffectivityDate.SendKeys(fe) );
            createNewTenantPage.Language.SendKeys(detail.Language);
            createNewTenantPage.SpanLanguage.Click();
            
        }

        [Given(@"I click the upload")]
        public void GivenIClickTheUpload()
        {
            createNewTenantPage.FileUpload.SendKeys(@"C:\Users\hsumabat\Pictures\logo-edenred.jpg");
            
        }

        [Given(@"I click the tenant save button")]
        public void GivenIClickTheTenantSaveButton()
        {
            createNewTenantPage.ClickSaveBtn();
        }

        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            Thread.Sleep(1500);
            string msg = createNewTenantPage.SuccessMessage.Text;
            Assert.True(msg == "Tenant has been successfully saved!");
        }

    }
}

public class TenantDetail
{
    public string TenantName { get; set; }
    public string Country { get; set; }
    public string TimeZone { get; set; }
    public string TimeFormat { get; set; }
    public decimal CompanyTaxRate { get; set; }
    public string EffectivityDate { get; set; }
    public string Language { get; set; }
}