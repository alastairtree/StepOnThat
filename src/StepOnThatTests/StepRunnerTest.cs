using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StepOnThat.Steps;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class StepRunnerTest
    {
        [Test]
        public async Task RunAsyncAddsDuration()
        {
            var run = new StepRunner();
            var result = await run.ExecuteStep(new Step());
            Thread.Sleep(1);
            Assert.True(result.Duration.TotalMilliseconds > 0);
        }
    }
}