﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System;
using StepOnThat.Infrastructure;
using Moq;
using Autofac;

namespace StepOnThat.Console.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public async Task MultiplePropertiesAreReadFromCommandLineIntoOptions()
        {
            var path = Path.GetTempFileName();
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
            var path = Path.GetTempFileName();
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
            var path = Path.GetTempFileName();
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
            var path = Path.GetTempFileName();
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
        [Category("WebBrowser")]
        public async Task ReadAndEchoValueFromTextBox()
        {
            var path = Path.GetTempFileName();
            const string instruction = @"{
                'steps': [
                    {
                        type: 'Browser',
                        url: 'http://www.google.com',
                        steps: [
                            { action: 'set', target: 'input[title=Search]', value: 'testSearch' },
                            { action: 'submit' },
                            { action: 'read', target: 'input[title=Search]', value: '${search-term}' },
                        ]
                    },
                    {
                        type:'echo', text:'${search-term}'
                    }
                ]
            }";
            File.WriteAllText(path, instruction);
            var dependenciesBuilder = new DependencyContainerBuilder();
            var builder = new ContainerBuilder();

            Mock<IOutput> output = new Mock<IOutput>();
            builder.RegisterInstance(output.Object).As<IOutput>();
            builder.Update(dependenciesBuilder.Container);


            var result =
                await Program.MainAsync(new[] { "--File", path }, dependenciesBuilder);
            File.Delete(path);

            Assert.True(result.Success);
            output.Verify(_ => _.Write(It.Is<string>(x => x == "testSearch")), Times.Once);
        }

        [Test]
        public void RunConsoleAppForAbadFilePath()
        {
            var path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'step', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = -1;
            var actual = Program.Main(new[] {"--File", path + "oops"});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RunConsoleAppForASingleBadStep()
        {
            var path = Path.GetTempFileName();
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
            var path = Path.GetTempFileName();
            const string instruction = @"{ steps : [{ type: 'Step', name: 'test' }] }";

            File.WriteAllText(path, instruction);

            const int expected = 0;
            var actual = Program.Main(new[] {"--File", path});

            File.Delete(path);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task SinglePropertiesAreReadFromCommandLineIntoOptions()
        {
            var path = Path.GetTempFileName();
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