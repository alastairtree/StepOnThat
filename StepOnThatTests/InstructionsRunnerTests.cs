using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsRunnerTests
    {
        [Test]
        public async Task RunTest()
        {
            var instructions = new[]
            {
                new Instructions()
                , new Instructions(new List<Step>())
                , new Instructions(new List<Step> {new Step {Name = "test", Type = "Test"}})
            };

            var runner = new InstructionsRunner(new StepRunner());

            foreach (Instructions instruction in instructions)
            {
                var expected = true;
                var actual = await runner.Run(instruction);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}