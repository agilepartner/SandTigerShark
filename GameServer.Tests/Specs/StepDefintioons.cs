namespace SandTigerShark.GameServer.Tests.Specs
{
    using TechTalk.SpecFlow;

    namespace Sample.Website.Tests
    {
        [Binding]
        public class StepDefinitions
        {
            [Given(@"I am curious")]
            public void GivenIAmCurious()
            {

            }

            [When(@"I request the version")]
            public void WhenIRequestTheVersion()
            {

            }

            [When(@"I yell '(.*)'")]
            public void WhenIYell(string exclamation)
            {
            }

            [Then(@"the result is content")]
            public void ThenTheResultIsContent()
            {
            }

            [Then(@"the result is constant")]
            public void ThenTheResultIsConstant()
            {
            }

            [Then(@"I hear '(.*)' echoed back")]
            public void ThenIHearEchoedBack(string exclamation)
            {
            }
        }
    }
}