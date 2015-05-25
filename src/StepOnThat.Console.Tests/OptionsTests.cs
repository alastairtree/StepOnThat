using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class OptionsTests
    {
        [Test]
        public void GetUsageAlwaysReturnsSomethingUSeful()
        {
            Assert.IsNotEmpty(new Options().GetUsage());
        }
    }
}