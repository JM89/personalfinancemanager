using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;
using System;
using TechTalk.SpecFlow;

namespace PersonalFinanceManager.IntegrationTests.Scenarios.Steps
{
    [Binding]
    public class CreateCountriesSteps
    {
        private int _countCountries;

        [Given(@"I have accessed the Country List page")]
        public void GivenIHaveAccessedTheCountryListPage()
        {
            _countCountries = DatabaseChecker.CountCountries();
            
            SiteMap.CountryListPage.GoTo();
        }
        
        [Given(@"I have clicked on the Create button")]
        public void GivenIHaveClickedOnTheCreateButton()
        {
            SiteMap.CountryListPage.ClickAddButton();
        }
        
        [When(@"I enter a name")]
        public void WhenIEnterAName()
        {
            SiteMap.CountryCreatePage.EnterName("Country Name");
        }
        
        [When(@"I click on the Save button")]
        public void WhenIClickOnTheSaveButton()
        {
            SiteMap.CountryCreatePage.ClickSave();
        }
        
        [Then(@"an country has been created")]
        public void ThenAnCountryHasBeenCreated()
        {
            var newCountCountries = DatabaseChecker.CountCountries();
            Assert.AreEqual(newCountCountries, _countCountries + 1);
        }
    }
}
