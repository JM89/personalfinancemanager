using BoDi;
using TechTalk.SpecFlow;

namespace PFM.IntegrationTests.Hooks
{
    [Binding]
    public class Hooks
    {
        private IObjectContainer _objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static async Task SetUp()
        {
      
        }

        [BeforeScenario]
        public void AddHttpClient()
        {
            var httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:4431") };

            _objectContainer.RegisterInstanceAs(httpClient);
        }

        [AfterTestRun]
        public static void TearDown()
        {

        }
    }
}
