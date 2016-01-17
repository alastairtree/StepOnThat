using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StepOnThat.Console.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public async Task MultiplePropertiesAreReadFromCommandLineIntoOptions()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";
            File.WriteAllText(path, instruction);

            var expected = "test2=123";
            var result = await Program.MainAsync(new[] {"--File", path, "-p", "test=abc", "test2=123"});
            File.Delete(path);

            Assert.AreEqual(2, result.Options.PropertyStrings.Length);
            Assert.AreEqual(expected, result.Options.PropertyStrings.Skip(1).First());
        }

        [Test]
        public async Task PropertiesFromCommandLineAreAddedToInstructions()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";
            File.WriteAllText(path, instruction);

            var expected = "abc";
            var result = await Program.MainAsync(new[] {"--File", path, "-p", "test=abc"});
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
            var result = await Program.MainAsync(new[] {"--File", path, "-p", "test=abc"});
            File.Delete(path);

            Assert.NotNull(result.Instructions.Properties);
            Assert.IsNotEmpty(result.Instructions.Properties);
            Assert.AreEqual(1, result.Instructions.Properties.Count);
            Assert.AreEqual(expected, result.Instructions.Properties["test"]);
        }

        [Test]
        [Category("WebBrowser")]
        public async Task ReadmeExampleReturnsSucessfully()
        {
            string path = Path.GetTempFileName();
            const string instruction = @"{
                'properties':[{key:'search-term', value:'hello world'}],
                'steps': [
                    {
                        type: 'Browser',
                        url: '${search-engine}',
                        steps: [
                            { action: 'set', target: 'input[title=Search]', value: '${search-term}' },
                            { action: 'submit' },
                            { action: 'title', match: '${search-term}*' },
                            { action: 'click', target: 'div[role=main] a:link' },
                        ]
                    }
                ]
            }";
            File.WriteAllText(path, instruction);

            var result =
                await Program.MainAsync(new[] {"--File", path, "--Properties", "search-engine=http://www.google.com"});
            File.Delete(path);

            Assert.True(result.Success);
        }

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
            var result = await Program.MainAsync(new[] {"--File", path, "-p", "test=abc"});
            File.Delete(path);

            Assert.NotNull(result.Options.PropertyStrings);
            Assert.IsNotEmpty(result.Options.PropertyStrings);
            Assert.AreEqual(expected, result.Options.PropertyStrings.SingleOrDefault());
        }
    }
}