using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepOnThat;
using NUnit.Framework;
namespace StepOnThat.Tests
{
    [TestFixture()]
    public class StepTests
    {
        [Test()]
        public void SimpleStepsAreEqual()
        {
            var expected = new Step() { Type = "Test" };
            var actual = new Step() { Type = "Test" };
            Assert.AreEqual(expected,actual);
        }
    }
}
