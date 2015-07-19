using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class OptionsTests
    {
        [Test]
        public void GetUsageAlwaysReturnsSomethingUSeful()
        {
            Assert.IsNotEmpty(Options.TryParse(new string[0]).GetUsage());
        }
    }
}