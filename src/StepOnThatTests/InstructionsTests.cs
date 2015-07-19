using System.Collections.Generic;
using NUnit.Framework;
using StepOnThat.Steps;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class InstructionsTests
    {
        [Test]
        public void DifferentInstructionsDifferentHashCode()
        {
            Assert.AreNotEqual(
                new Instructions(new[] {new Step {Name = "test"}}).GetHashCode(),
                new Instructions(new[] {new Step {Name = "test2"}}).GetHashCode()
                );
        }

        [Test]
        public void DumbEmptyInstructionsAreEqual()
        {
            var actual = new Instructions(new List<Step>());
            var expected = new Instructions(new List<Step>());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SameInstructionsSameHashCode()
        {
            Assert.AreEqual(
                new Instructions(new[] {new Step {Name = "test"}}).GetHashCode(),
                new Instructions(new[] {new Step {Name = "test"}}).GetHashCode()
                );
        }
    }
}