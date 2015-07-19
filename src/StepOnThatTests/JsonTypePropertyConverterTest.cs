using Newtonsoft.Json;
using NUnit.Framework;
using StepOnThat.Infrastructure;
using StepOnThat.Steps.Browser.Actions;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class JsonTypePropertyConverterTest
    {
        [Test]
        public void DeserialseBasedOnActionProperty()
        {
            var json = "{action:'goto',url:'http://example.com'}";
            var container = new DependencyContainerBuilder();
            var ins = new InstructionTypeFactory(container.Container);
            var sut = new JsonTypePropertyConverter<BrowserAction>(ins, typePropertyDiscriminatorName: "action");

            var action = (GoTo) JsonConvert.DeserializeObject<BrowserAction>(json, sut);
            Assert.AreEqual("http://example.com", action.Url);
        }
    }
}