using Newtonsoft.Json;
using NUnit.Framework;
using StepOnThat.Browser.Actions;
using StepOnThat.Infrastructure;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class JsonTypePropertyConverterTest
    {
        [Test]
        public void DeserialseBasedOnActionProperty()
        {
            var json = "{action:'goto',url:'http://example.com'}";
            var container = new DependencyResolver();
            var sut = new JsonTypePropertyConverter<BrowserAction>(container.Container, typePropertyName: "action");

            var action = (GoTo) JsonConvert.DeserializeObject<BrowserAction>(json, sut);
            Assert.AreEqual("http://example.com", action.Url);
        }
    }
}