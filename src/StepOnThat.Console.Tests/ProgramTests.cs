using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";

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
            const string instruction = @"{ steps : [{ type: 'step', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = -1;
            var actual = Program.Main(new[] {"--File", path + "oops"});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SinglePropertiesAreReadFromCommandLineIntoOptions()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";
            File.WriteAllText(path, instruction);

            var expected = "test=abc";
            var result = await Program.MainAsync(new[] { "--File", path, "-p", "test=abc" });
            File.Delete(path);

            Assert.NotNull(result.Options.Properties);
            Assert.IsNotEmpty(result.Options.Properties);
            Assert.AreEqual(expected, result.Options.Properties.SingleOrDefault());
        }

        [Test]
        public async Task MultiplePropertiesAreReadFromCommandLineIntoOptions()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";
            File.WriteAllText(path, instruction);

            var expected = "test2=123";
            var result = await Program.MainAsync(new[] { "--File", path, "-p", "test=abc", "test2=123" });
            File.Delete(path);

            Assert.AreEqual(2,result.Options.Properties.Length);
            Assert.AreEqual(expected, result.Options.Properties.Skip(1).First());
        }

        [Test]
        public async Task PropertiesFromCommandLineAreAddedToInstructions()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";
            File.WriteAllText(path, instruction);

            var expected = "abc";
            var result = await Program.MainAsync(new[] { "--File", path, "-p", "test=abc" });
            File.Delete(path);

            Assert.NotNull(result.Instructions.Properties);
            Assert.IsNotEmpty(result.Instructions.Properties);
            Assert.AreEqual(expected, result.Instructions.Properties["test"]);
        }

        [Test]
        public async Task PropertiesFromCommandLineAreAddedToInstructionsAndOverrideJsonFile()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ 
                properties: [{key:'test', value:'not-valid'}],
                steps : [{ type: 'Step', name: 'test' }] 
            }";
            File.WriteAllText(path, instruction);

            var expected = "abc";
            var result = await Program.MainAsync(new[] { "--File", path, "-p", "test=abc" });
            File.Delete(path);

            Assert.NotNull(result.Instructions.Properties);
            Assert.IsNotEmpty(result.Instructions.Properties);
            Assert.AreEqual(1, result.Instructions.Properties.Count);
            Assert.AreEqual(expected, result.Instructions.Properties["test"]);
        }
    }
}