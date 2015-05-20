using System.Collections.Generic;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsTests
    {
        [Test]
        public void DumbEmptyInstructionsAreEqual()
        {
            var actual = new Instructions(new List<Step>());
            var expected = new Instructions(new List<Step>());
            Assert.AreEqual(expected, actual);
        }
    }
}