using NUnit.Framework;
using System.Threading.Tasks;

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
        public async Task RunAsyncTesthasNoError()
        {
            var step = new Step();
            IStepResult result = await step.RunAsync();
            Assert.True(result.Error == null);
        }

        [Test]
        public void SimpleStepsAreEqual()
        {
            var expected = new Step {Type = "Test", Name = "Test"};
            var actual = new Step {Type = "Test", Name = "Test"};
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleStepsAreNotEqualByName()
        {
            var expected = new Step { Type = "Test", Name = "Test" };
            var actual   = new Step { Type = "Test", Name = "NotTest" };
            Assert.AreNotEqual(expected, actual);

            expected = new Step { Type = "Test", Name = "Test" };
            actual = new Step { Type = "TestName", Name = "Test" };
            Assert.AreNotEqual(expected, actual);
        }
    }
}