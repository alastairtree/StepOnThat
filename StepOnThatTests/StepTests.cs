using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class StepTests
    {
        [Test]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(new Step {Name = "test"}.GetHashCode(), new Step {Name = "test"}.GetHashCode());
            Assert.AreNotEqual(new Step {Name = "test"}.GetHashCode(), new Step {Name = "test2"}.GetHashCode());
            Assert.AreNotEqual(new Step {Name = "test"}.GetHashCode(), new Step {Type = "test"}.GetHashCode());
        }

        [Test]
        public async void RunAsyncTestSuceeds()
        {
            var step = new Step();
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
        }

        [Test]
        public async Task RunAsyncTesthasDuration()
        {
            var step = new Step();
            IStepResult result = await step.RunAsync();
            Thread.Sleep(1);
            Assert.True(result.Duration.TotalMilliseconds > 0);
        }

        [Test]
        public void SimpleStepsAreEqual()
        {
            var expected = new Step {Type = "Test", Name = "Test"};
            var actual = new Step {Type = "Test", Name = "Test"};
            Assert.AreEqual(expected, actual);
        }
    }
}