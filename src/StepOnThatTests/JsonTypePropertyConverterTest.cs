using Newtonsoft.Json;
using NUnit.Framework;
using StepOnThat.Browser.Actions;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class JsonTypePropertyConverterTest
    {
        [Test]
        public void DeserialseBasedOnActionProperty()
        {
            var json = "{action:'goto',url:'http://example.com'}";
            var sut = new JsonTypePropertyConverter<BrowserAction>(typePropertyName: "action");

            var action = (GoTo) JsonConvert.DeserializeObject<BrowserAction>(json, sut);
            Assert.AreEqual("http://example.com", action.Url);
        }
    }
}