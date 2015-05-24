using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsReaderWriterTests
    {
        private static void WriteAndTheReadInstructions(Instructions instructions)
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
                WriteAndTheReadInstructions(instruction);
            }
        }

        [Test]
        public void EmptyInstructionsFromAJsonString()
        {
            var json = "{steps:[]}";
            var instruction = InstructionsReaderWriter.Read(json);
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsEmpty(instruction.Steps);
        }

        [Test]
        public void SingleEmptyStepIsNotNull()
        {
            var json = "{steps:[{}]}";
            var instruction = InstructionsReaderWriter.Read(json);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.NotNull(instruction.Steps.Single());
        }


        [Test]
        public void EmptyInstructionsFromAnEmptyPairOfBraces()
        {
            var json = "{}";
            var instruction = InstructionsReaderWriter.Read(json);
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
        }

        [TestCase("{steps:[{name:'test'}]}")]
        [TestCase("{steps:[{name:'test', type:'step'}]}")]
        [TestCase("{steps:[{name:'test', type:'Step'}]}")]
        public void ReadInstructionsFromAJsonString(string json)
        {
            var instruction = InstructionsReaderWriter.Read(json);
            var expected = "test";
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.AreEqual(expected, instruction.Steps[0].Name);
        }
    }
}