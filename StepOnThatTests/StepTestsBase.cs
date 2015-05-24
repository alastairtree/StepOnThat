using System.Threading.Tasks;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    public class StepTestsBase<TStep> where TStep : Step, new()
    {
        protected virtual TStep GetStepForTesting(string name = "testName")
        {
            return new TStep {Name = name};
        }

        [Test]
        public void DifferentNameDifferentHashCode()
        {
            Assert.AreNotEqual(GetStepForTesting().GetHashCode(), GetStepForTesting("test2").GetHashCode());
        }

        [Test]
        public void SameNameSameHashCode()
        {
            Assert.AreEqual(GetStepForTesting().GetHashCode(), GetStepForTesting().GetHashCode());
        }

        [Test]
        public async void RunAsyncTestSuceeds()
        {
            var step = GetStepForTesting();
            IStepResult result = await step.RunAsync();
            Assert.True(result.Success);
        }

        [Test]
        public async Task RunEmptyAsyncTestHasNoError()
        {
            var step = GetStepForTesting();
            IStepResult result = await step.RunAsync();
            Assert.True(result.Error == null);
        }

        [Test]
        public void SimpleStepsAreEqual()
        {
            var expected = GetStepForTesting();
            var actual = GetStepForTesting();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleStepsAreNotEqualByName()
        {
            var expected = GetStepForTesting();
            var actual = GetStepForTesting("test2");
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void TypeIsEqualToClassName()
        {
            var actual = GetStepForTesting().Type;
            var expected = typeof (TStep).Name;
            Assert.AreEqual(expected, actual);
        }
    }
}