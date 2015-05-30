using System.Threading.Tasks;
using NUnit.Framework;
using StepOnThat.Tests;

namespace StepOnThat.Http.Tests
{
    [TestFixture]
    public class HttpStepTests : StepTestsBase<HttpStep>
    {
        public static string testRequestUrl = "http://httpbin.org/";

        [Test]
        public async Task RunSimpleGetHttpHasData()
        {
            var step = GetStepForTesting();
            step.Url = testRequestUrl;
            var result = (HttpStepResult) await step.RunAsync();
            Assert.True(result.Response.IsSuccessStatusCode);
        }

        [Test]
        public async Task RunSimpleGetHttpSuceeds()
        {
            var step = GetStepForTesting();
            step.Url = testRequestUrl;
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
            Assert.True(result.Error == null);
        }

        [Test]
        public async Task SendAPostWithMessageToRequestBin()
        {
            var step = GetStepForTesting();
            step.Url = testRequestUrl + "post";
            step.Method = "POST";
            step.Data = "{'message':'hello api!'}";
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
            Assert.True(result.Error == null);
        }
    }
}