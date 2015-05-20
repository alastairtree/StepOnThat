using System.IO;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsReaderWriterTests
    {
        [Test]
        public void WriteFileTheReadItIsEqual()
        {
            var instructions = new Instructions();
            string path = Path.GetTempFileName();
            InstructionsReaderWriter.WriteFile(instructions, path);
            Instructions clone = InstructionsReaderWriter.ReadFile(path);
            Assert.AreEqual(instructions, clone);
        }
    }
}