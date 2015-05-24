using System.Threading.Tasks;
using NUnit.Framework;
using StepOnThat.Tests;

namespace StepOnThat.Http.Tests
{
    [TestFixture]
    public class HttpStepTests : StepTestsBase<HttpStep>
    {
        [Test]
        public async Task RunSimpleGetHttpHasData()
        {
            var step = GetStepForTesting();
            step.Url = "http://www.bbc.com";
            var result = (HttpStepResult) await step.RunAsync();
            Assert.True(result.Data.Length > 10);
        }

        [Test]
        public async Task RunSimpleGetHttpSuceeds()
        {
            var step = GetStepForTesting();
            step.Url = "http://www.bbc.com";
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
            Assert.True(result.Error == null);
        }
    }
}