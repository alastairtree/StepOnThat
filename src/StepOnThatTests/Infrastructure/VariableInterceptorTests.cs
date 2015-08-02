using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Moq;
using NUnit.Framework;
using StepOnThat.Steps.Browser;

namespace StepOnThat.Infrastructure.Tests
{
    [TestFixture]
    public class VariableInterceptorTests
    {
        private InstructionsReaderWriter GetReaderWriter(ILifetimeScope scope)
        {
            var builder = new InstructionTypeFactory(scope);
            return new InstructionsReaderWriter(builder);
        }

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

        [Test]
        public void VariablesCanBeWritttenToBySteps()
        {
            var dependencies = new DependencyContainerBuilder();
            using (var scope = dependencies.Container.BeginLifetimeScope())
            {
                var builder = new ContainerBuilder();
                var browser = new Mock<IWebBrowser>();
                browser.Setup(x => x.Get(It.IsAny<string>())).Returns("rightvalue");
                builder.RegisterInstance(browser.Object).AsImplementedInterfaces();
                builder.Update(dependencies.Container);

                var variables = scope.Resolve<IHasProperties>();
                var reader = GetReaderWriter(scope);
                var runner = scope.Resolve<IInstructionsRunner>();

                var ins = reader.Read(@"{
                    'properties':[{
                        'key':'anotherVariable', 
                        'value':'wrongvalue'
                    }], 
                    'steps':[{
                        'type':'browser',
                        'url':'http://www.google.com',
                        'steps':[{'action':'read','target':'.username', 'value':'${variableundertest}'}]
                    }]                        
                }");

                Task.WaitAll(runner.Run(ins));

                Assert.AreEqual("rightvalue", variables["variableundertest"]);
            }
        }
    }
}