using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsReaderWriterTests
    {
        private static void WriteTheReadAndAssertItIsEqual(Instructions instructions)
        {
            string path = Path.GetTempFileName();
            InstructionsReaderWriter.WriteFile(instructions, path);
            Instructions clone = InstructionsReaderWriter.ReadFile(path);
            Assert.AreEqual(instructions, clone);
            File.Delete(path);
        }

        [Test]
        public void WriteFileThenReadAndCheckItIsEqual()
        {
            var instructions = new[]
            {
                new Instructions()
                , new Instructions(new List<Step>())
                , new Instructions(new List<Step> {new Step { Name = "test" }})
            };

            foreach (Instructions instruction in instructions)
            {
                WriteTheReadAndAssertItIsEqual(instruction);
            }
        }
    }
}