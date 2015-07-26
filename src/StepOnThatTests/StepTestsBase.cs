using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using StepOnThat.Infrastructure;
using StepOnThat.Steps;

namespace StepOnThat.Tests
{
    public class StepTestsBase<TStep> where TStep : Step
    {
        private ILifetimeScope injector;
        private readonly DependencyContainerBuilder containerBuilder = new DependencyContainerBuilder(false);

        protected virtual void OverrideContainerRegistrations(ContainerBuilder builder)
        {
            
        }

        [SetUp]
        public virtual void Before()
        {
            containerBuilder.Init();
            containerBuilder.Build();

            var builder = new ContainerBuilder();
            OverrideContainerRegistrations(builder);
            builder.Update(containerBuilder.Container);

            injector = containerBuilder.Container.BeginLifetimeScope();
        }

        [TearDown]
        public virtual void After()
        {
            injector.Dispose();
        }


        protected virtual TStep GetStepForTesting(string name = "testName")
        {
            var step = injector.Resolve<TStep>();
            step.Name = name;
            return step;
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