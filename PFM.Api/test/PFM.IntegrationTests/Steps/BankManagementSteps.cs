using Api.Contracts.Shared;
using Newtonsoft.Json;
using PFM.Bank.Api.Contracts.Bank;
using PFM.IntegrationTests.TestSetup;
using System.Net.Http.Json;
using TechTalk.SpecFlow;

namespace PFM.IntegrationTests.Steps
{
    [Binding]
    public class BankManagementSteps
    {
        private readonly HttpClient _httpClient;
        private readonly ScenarioContext _scenarioContext;

        public BankManagementSteps(HttpClient httpClient, ScenarioContext scenarioContext)
        {
            _httpClient = httpClient;
            _scenarioContext = scenarioContext;
        }

        [When(@"I provide valid bank details")]
        public async Task WhenIProvideValidBankDetails()
        {
            var bank = await _httpClient.CreateNewBank();

            _scenarioContext.Add("BankIdCreated", bank.Id);

            Assert.NotNull(bank);
        }

        [Then(@"The banks are created successfully")]
        public async Task ThenTheBanksAreCreatedSuccessfully()
        {
            var id = _scenarioContext.Get<int>("BankIdCreated");

            var response = await _httpClient.GetAsync($"/api/Bank/{id}");
            var responseApiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            var data = JsonConvert.DeserializeObject<BankDetails>(responseApiResponse?.Data.ToString() ?? "");

            Assert.NotNull(data);
        }
    }
}
