using System.Threading.Tasks;
using Autofac;
using Moq;
using NUnit.Framework;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class EchoTests : StepTestsBase<Echo>
    {
        private readonly Mock<IOutput> output = new Mock<IOutput>();

        protected override void OverrideContainerRegistrations(ContainerBuilder builder)
        {
            builder.RegisterInstance(output.Object).As<IOutput>();
        }

        [Test]
        public async Task EchoNothingDoesNothing()
        {
            var sut = GetStepForTesting();

            await sut.RunAsync();

            output.Verify(_ => _.Write(It.IsAny<string>()), Times.Never());
            output.Verify(_ => _.Write(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never());
        }

        [Test]
        public async Task EchoTextIsSentToOutput()
        {
            var sut = GetStepForTesting();
            sut.Text = "Hello";

            await sut.RunAsync();

            output.Verify(_ => _.Write(It.Is<string>(x => x == "Hello")), Times.Once);
        }
    }
}