using System.IO;
using NUnit.Framework;

namespace StepOnThat.Console.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void RunConsoleAppForASingleBadStep()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : OOPS [{ type: 'test', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = -1;
            var actual = Program.Main(new[] {"--File", path});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RunConsoleAppForASingleNoOpStep()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'test', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = 0;
            var actual = Program.Main(new[] {"--File", path});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RunConsoleAppForAbadFilePath()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'test', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = -1;
            var actual = Program.Main(new[] {"--File", path + "oops"});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }
    }
}