using TechTalk.SpecFlow;

namespace SandTigerShark.GameServer.Tests.Specs
{
    [Binding]
    public class StepDefinitions
    {
        [Given(@"I am curious")]
        public void GivenIAmCurious()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I request the version")]
        public void WhenIRequestTheVersion()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I yell '(.*)'")]
        public void WhenIYell(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result is content")]
        public void ThenTheResultIsContent()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I hear '(.*)' echoed back")]
        public void ThenIHearEchoedBack(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
