using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using StepOnThat.Browser.Actions;
using StepOnThat.Http;
using StepOnThat.Tests.Browser;

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

        [TestCase("[{name:'test'}]")]
        [TestCase("{steps:[{name:'test'}]}")]
        [TestCase("{steps:[{name:'test', type:'step'}]}")]
        [TestCase("{steps:[{name:'test', type:'Step'}]}")]
        public void ReadStepInstructionsFromAJsonString(string json)
        {
            var instruction = InstructionsReaderWriter.Read(json);
            var expected = "test";
            Assert.NotNull(instruction);
            Assert.NotNull(instruction.Steps);
            Assert.IsNotEmpty(instruction.Steps);
            Assert.AreEqual(expected, instruction.Steps[0].Name);
        }

        [TestCase("{steps:[{name:'test', type:'HttpStep'}]}")]
        [TestCase("{steps:[{name:'test', type:'httpstep'}]}")]
        public void ReadHttpStepInstructionsFromAJsonString(string json)
        {
            var instruction = InstructionsReaderWriter.Read(json);
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
            var instruction = InstructionsReaderWriter.Read(json);
            var expected = "HttpStep";
            Assert.AreEqual(expected, instruction.Steps[0].Type);
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
            var instruction = InstructionsReaderWriter.Read(json);
            Assert.AreEqual(typeof (BrowserStep), instruction.Steps[0].GetType());

            var browserStep = (BrowserStep) instruction.Steps[0];
            Assert.AreEqual(2, browserStep.Steps.Count);

            var action = (GoTo) browserStep.Steps.First();
            Assert.AreEqual("http://example.com/url", action.Url);

            var action2 = (Click) browserStep.Steps.Last();
            Assert.AreEqual("a:link", action2.Target);
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
        public void EmptyInstructionsFromAnEmptyPairOfBraces()
        {
            var json = "{}";
            var instruction = InstructionsReaderWriter.Read(json);
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
            var instruction = InstructionsReaderWriter.Read(json);
            Assert.AreEqual("1", instruction.Steps[0].Name);
            Assert.AreEqual("2", instruction.Steps[1].Name);
            Assert.AreEqual(typeof (HttpStep), instruction.Steps[1].GetType());
            Assert.AreEqual("3", instruction.Steps[2].Name);
            Assert.AreEqual(typeof (HttpStep), instruction.Steps[2].GetType());
            Assert.AreEqual("4", instruction.Steps[3].Name);
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
        [ExpectedException(typeof (ApplicationException))]
        public void UnknownStepTypeFailsToDeserialise()
        {
            var json = "{steps:[{type:'someUnknownType'}]}";
            var instruction = InstructionsReaderWriter.Read(json);
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
                WriteAndTheReadInstructions(instruction);
            }
        }
    }
}