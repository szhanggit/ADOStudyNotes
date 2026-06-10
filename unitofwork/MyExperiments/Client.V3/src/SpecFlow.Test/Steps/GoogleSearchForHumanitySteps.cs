using SpecFlow.Test.Drivers;
using SpecFlow.Test.Pages.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace SpecFlow.Test.Steps
{
    [Binding]
    public sealed class GoogleSearchForHumanitySteps
    {
        private readonly SearchEnginePage searchEnginePage;
        private readonly SeleniumDriver seleniumDriver;
        public GoogleSearchForHumanitySteps(SeleniumDriver seleniumDriver)
        {
            this.seleniumDriver = seleniumDriver;
            searchEnginePage = new SearchEnginePage(seleniumDriver);
        }

        [Given(@"I launch the google")]
        public void GivenILaunchTheGoogle()
        {
            seleniumDriver.WebDriver.Navigate().GoToUrl("https://www.google.com/");
        }

        [Given(@"I entery humanity")]
        public void GivenIEnteryHumanity()
        {
            searchEnginePage.SearchFor("Humanity");
        }

        [Given(@"I click the search button")]
        public void GivenIClickTheSearchButton()
        {
            searchEnginePage.SubmitSearch();
        }

        

        [Then(@"the result should be '(.*)'")]
        public void ThenTheResultShouldBe(string searchValue)
        {
            Assert.Equal(seleniumDriver.WebDriver.Title, searchValue);
        }


    }
}
