using NUnit.Framework;
using StepOnThat.Steps;
using StepOnThat.Steps.Http;
using StepOnThat.Tests;
using System.Threading.Tasks;

namespace StepOnThat.Http.Tests
{
    [TestFixture]
    public class HttpStepTests : StepTestsBase<HttpStep>
    {
        public static string testRequestUrl = "http://httpbin.org/";

        [Test]
        [Category("MakesHttpCalls")]
        public async Task RunSimpleGetHttpHasData()
        {
            var step = GetStepForTesting();
            step.Url = testRequestUrl;
            var result = (HttpStepResult) await step.RunAsync();
            Assert.True(result.Response.IsSuccessStatusCode);
        }

        [Test]
        [Category("MakesHttpCalls")]
        public async Task RunSimpleGetHttpSuceeds()
        {
            var step = GetStepForTesting();
            step.Url = testRequestUrl;
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
            Assert.True(result.Error == null);
        }

        [Test]
        [Category("MakesHttpCalls")]
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