using NUnit.Framework;
using StepOnThat.Tests;
using System.Threading.Tasks;

namespace StepOnThat.Http.Tests
{
    [TestFixture]
    public class HttpStepTests : StepTestsBase<HttpStep>
    {
        [Test]
        public async Task RunSimpleGetHttpSuceeds()
        {
            var step = GetStepForTesting();
            step.Url = "http://www.bbc.com";
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
            Assert.True(result.Error == null);
        }

        [Test]
        public async Task RunSimpleGetHttpHasData()
        {
            var step = GetStepForTesting();
            step.Url = "http://www.bbc.com";
            HttpStepResult result = (HttpStepResult) await step.RunAsync();
            Assert.True(result.Data.Length > 10);
        }
    }
}