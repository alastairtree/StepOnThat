using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NUnit.Framework;
using StepOnThat.Browser.Actions;
using StepOnThat.Http;
using StepOnThat.Infrastructure;
using StepOnThat.Tests.Browser;
using StepOnThat.Http.Tests;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsReaderWriterTests
    {
        private DependencyResolver resolver;
        private InstructionsReaderWriter readerWriter;

        [SetUp]
        public void BeforeEachTest()
        {
            resolver = new DependencyResolver();
            readerWriter = new InstructionsReaderWriter(resolver.Container);
        }

        private void WriteAndThenReadInstructions(Instructions instructions)
        {
            string path = Path.GetTempFileName();
            readerWriter.WriteFile(instructions, path);
            Instructions clone = readerWriter.ReadFile(path);
            Assert.AreEqual(instructions, clone);
            File.Delete(path);
        }

        [TestCase("[{name:'test'}]")]
        [TestCase("{steps:[{name:'test'}]}")]
        [TestCase("{steps:[{name:'test', type:'step'}]}")]
        [TestCase("{steps:[{name:'test', type:'Step'}]}")]
        [TestCase("{steps:[{name:'test', type:'browser'}]}")]
        public void ReadStepInstructionsFromAJsonString(string json)
        {
            var instruction = readerWriter.Read(json);
            var expected = "test";
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.AreEqual(expected, instruction.Steps[0].Name);
        }

        [TestCase("{steps:[{name:'test', type:'Http'}]}")]
        [TestCase("{steps:[{name:'test', type:'HttpStep'}]}")]
        [TestCase("{steps:[{name:'test', type:'httpstep'}]}")]
        public void ReadHttpStepInstructionsFromAJsonString(string json)
        {
            var instruction = readerWriter.Read(json);
            var expected = "test";
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.AreEqual(expected, instruction.Steps[0].Name);
        }

        [TestCase("{steps:[{type:'HttpStep'}]}")]
        [TestCase("{steps:[{name:'test', type:'HttpStep'}]}")]
        [TestCase("{steps:[{name:'test', type:'httpstep'}]}")]
        [TestCase("{steps:[{name:'test', type:'httpstep', url:'http://www.example.com'}]}")]
        public void ReadingHttpStepReturnsTheCorrectType(string json)
        {
            var instruction = readerWriter.Read(json);
            Assert.True(instruction.Steps[0].IsTypeOf<HttpStep>());
        }

        [Test]
        [Category("WebBrowser")]
        public void BrowserStepWithActionsIsDeserialisedCorrectly()
        {
            var json = @"{
                steps:[
                    {
                        type:'BrowserStep', 
                        url:'http://example.com',
                        steps:[
                            {action:'goto',url:'http://example.com/url'},
                            {action:'click',target:'a:link'},
                        ]
                    },
                ]
            }";
            var instruction = readerWriter.Read(json);
            Assert.True(instruction.Steps[0].IsTypeOf<BrowserStep>());

            var browserStep = (BrowserStep) instruction.Steps[0];
            Assert.AreEqual(2, browserStep.Steps.Count);

            var action = (GoTo) browserStep.Steps.First();
            Assert.AreEqual("http://example.com/url", action.Url);

            var action2 = (Click) browserStep.Steps.Last();
            Assert.AreEqual("a:link", action2.Target);
        }


        [Test]
        [Category("WebBrowser")]
        public async Task ComplexBrowserActionsDeserialiseAndRun()
        {
            var json = @"
            {
                'steps': [
                    {
                        type: 'Browser',
                        url: 'http://www.google.com',
                        steps: [
                            { action: 'set', target: 'input[title=Search]', value: 'hello world' },
                            { action: 'submit' },
                            { action: 'title', match: 'hello world*' },
                            { action: 'click', target: 'div[role=main] a:link' },
                            { action: 'waitfor', target: '.thumb img' },
                            { action: 'address', match: '*.wikipedia.*' },
                        ]
                    },
                    {
                        type: 'Http', 
                        url: '" + HttpStepTests.testRequestUrl + @"', 
                        method: 'post', 
                        data : '{message:\""hello APIs\""}'
                    }
                ]
            }";
            var instruction = readerWriter.Read(json);
            Assert.True(instruction.Steps[0].IsTypeOf<BrowserStep>());

            var browserStep = (BrowserStep) instruction.Steps[0];
            Assert.AreEqual(6, browserStep.Steps.Count);

            await new InstructionsRunner(new StepRunner()).Run(instruction);
        }
        

        [Test]
        public void EmptyInstructionsFromAJsonString()
        {
            var json = "{steps:[]}";
            var instruction = readerWriter.Read(json);
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsEmpty(instruction.Steps);
        }

        [Test]
        public void EmptyInstructionsFromAnEmptyPairOfBraces()
        {
            var json = "{}";
            var instruction = readerWriter.Read(json);
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
        }

        [Test]
        public void ManyDifferentTypesOfStepAreReadInTheCorrectOrderAndWithCorrectTypes()
        {
            var json = @"{
                steps:[
                    {name:1},
                    {name:2,   type:'httpStep'},
                    {name:3,   type:'httpStep', url:'http://example.com'},
                    {name:'4', type:'step'}
                ]
            }";
            var instruction = readerWriter.Read(json);
            Assert.AreEqual("1", instruction.Steps[0].Name);
            Assert.AreEqual("2", instruction.Steps[1].Name);
            Assert.AreEqual(new HttpStep(new HttpClient()) { Name = "2" }, instruction.Steps[1]);
            Assert.AreEqual("3", instruction.Steps[2].Name);
            Assert.True(instruction.Steps[2].IsTypeOf<HttpStep>());
            Assert.AreEqual("4", instruction.Steps[3].Name);
        }

        [Test]
        public void SingleEmptyStepIsNotNull()
        {
            var json = "{steps:[{}]}";
            var instruction = readerWriter.Read(json);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.NotNull(instruction.Steps.Single());
        }

        [Test]
        [ExpectedException(typeof (ApplicationException))]
        public void UnknownStepTypeFailsToDeserialise()
        {
            var json = "{steps:[{type:'someUnknownType'}]}";
            var instruction = readerWriter.Read(json);
        }

        [Test]
        public void WriteFileThenReadAndCheckItIsEqual()
        {
            var instructions = new[]
            {
                new Instructions()
                , new Instructions(new List<Step>())
                , new Instructions(new List<Step> {new Step {Name = "test"}})
            };

            foreach (Instructions instruction in instructions)
            {
                WriteAndThenReadInstructions(instruction);
            }
        }
    }
}