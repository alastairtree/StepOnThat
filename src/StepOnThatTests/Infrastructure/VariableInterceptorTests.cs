﻿using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;

namespace StepOnThat.Infrastructure.Tests
{
    [TestFixture]
    public class VariableInterceptorTests
    {
        [Test]
        public async Task StepsWithVariablesRun
            ()
        {
            var resolver = new DependencyResolver();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var variables = scope.Resolve<IVariables>();

                variables["test"] = "EvaluatedAsAVariable";

                var reader = new InstructionsReaderWriter(scope);

                var ins = reader.Read(@"[{type:'Step',Name:'${test}'}]");

                var runner = scope.Resolve<IInstructionsRunner>();

                var result = await runner.Run(ins);

                Assert.IsTrue(result);
            }
        }

        [Test]
        public void VariablesAreInterceptedAndReturned()
        {
            var resolver = new DependencyResolver();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var variables = scope.Resolve<IVariables>();

                variables["test"] = "EvaluatedAsAVariable";

                var reader = new InstructionsReaderWriter(scope);

                var ins = reader.Read(@"[{type:'Step',Name:'${test}'}]");

                Assert.AreEqual("EvaluatedAsAVariable", ins.Steps.First().Name);
            }
        }
    }
}