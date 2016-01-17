using Autofac;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace StepOnThat.Infrastructure.Tests
{
    [TestFixture]
    public class VariableInterceptorTests
    {
        [Test]
        public void PropertiesAreDeserialised()
        {
            var resolver = new DependencyContainerBuilder();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var reader = GetReaderWriter(scope);

                var ins =
                    reader.Read(
                        @"{'properties':[{'key':'test', 'value':'EvaluatedAsAVariable'}], 'steps': [{type:'Step',Name:'${test}'}] }");

                Assert.IsNotNull(ins.Properties);
                Assert.IsNotEmpty(ins.Properties);
                Assert.AreEqual("EvaluatedAsAVariable", ins.Properties["test"]);
                Assert.AreEqual("EvaluatedAsAVariable", ins.Properties[0].Value);
            }
        }

        private InstructionsReaderWriter GetReaderWriter(ILifetimeScope scope)
        {
            var builder = new InstructionTypeFactory(scope);
            return new InstructionsReaderWriter(builder);
        }

        [Test]
        public async Task StepsWithVariablesRun
            ()
        {
            var resolver = new DependencyContainerBuilder();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var variables = scope.Resolve<IHasProperties>();

                variables["test"] = "EvaluatedAsAVariable";

                var reader = GetReaderWriter(scope);

                var ins = reader.Read(@"[{type:'Step',Name:'${test}'}]");

                var runner = scope.Resolve<IInstructionsRunner>();

                var result = await runner.Run(ins, stepResults: null);

                Assert.IsTrue(result);
            }
        }

        [Test]
        public void VariablesAreInterceptedAndReturned()
        {
            var resolver = new DependencyContainerBuilder();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var variables = scope.Resolve<IHasProperties>();

                variables["test"] = "EvaluatedAsAVariable";

                var reader = GetReaderWriter(scope);

                var ins = reader.Read(@"[{type:'Step',Name:'${test}'}]");

                Assert.AreEqual("EvaluatedAsAVariable", ins.Steps.First().Name);
            }
        }

        [Test]
        public void VariablesCanBeAttachedToInstructions()
        {
            var resolver = new DependencyContainerBuilder();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var reader = GetReaderWriter(scope);

                var ins =
                    reader.Read(
                        @"{'properties':[{'key':'test', 'value':'EvaluatedAsAVariable'}], 'steps': [{type:'Step',Name:'${test}'}] }");

                Assert.AreEqual("EvaluatedAsAVariable", ins.Steps.First().Name);
            }
        }
    }
}