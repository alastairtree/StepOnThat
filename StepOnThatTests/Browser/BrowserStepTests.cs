using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StepOnThat.Browser;
using StepOnThat.Browser.Actions;

namespace StepOnThat.Tests.Browser
{
    [TestFixture]
    public class BrowserStepTests
    {
        public static string testHomepage = "http://example.com/";

        [Test]
        public void BasicBrowserStepInitialisesProperties()
        {
            var browser = new Mock<IWebBrowser>();
            var sut = new BrowserStep(browser.Object, testHomepage);
            Assert.AreEqual(testHomepage, sut.Url);

            ICollection<BrowserAction> steps = sut.Steps; 
            Assert.NotNull(steps);
            Assert.IsEmpty(steps);
        }

        [Test]
        public async Task BasicBrowserStepOpensOnAUrl()
        {
            var browser = new Mock<IWebBrowser>();
            var sut = new BrowserStep(browser.Object, testHomepage);

            await sut.RunAsync();

            browser.Verify(b => b.GoTo(testHomepage), Times.Once);
        }

        [Test]
        public async Task BasicBrowserStepWithoutUrlDoesNothing()
        {
            var browser = new Mock<IWebBrowser>();
            var sut = new BrowserStep(browser.Object);

            await sut.RunAsync();

            browser.Verify(b => b.GoTo(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task SingleStepIsExecutedAfterFirstUrl()
        {
            var browser = new Mock<IWebBrowser>();
            var sut = new BrowserStep(browser.Object, testHomepage);
            var secondUrl = testHomepage + "somePath";
            sut.Steps.Add(new GoTo {Url = secondUrl});

            await sut.RunAsync();

            browser.Verify(b => b.GoTo(testHomepage), Times.Once);
            browser.Verify(b => b.GoTo(secondUrl), Times.Once);
        }
    }
}
