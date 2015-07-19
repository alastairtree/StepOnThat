using NUnit.Framework;
using StepOnThat.Steps.Browser;

namespace StepOnThat.Browser.Tests
{
    [TestFixture]
    [Category("WebBrowser")]
    public class WebBrowserTests
    {
        private string homepageUrl = "http://www.bbc.co.uk";
        private string searchboxCss = "input[type=text][name=q]";
        private WebBrowser browser;

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {
            browser = new WebBrowser();
        }

        [TestFixtureTearDown]
        public void AfterAlltests()
        {
            browser.Close();
            browser = null;
        }

        [Test]
        public void BackAndForwardTest()
        {
            browser
                .GoTo(homepageUrl)
                .Click("a:link")
                .Back()
                .Forward()
                .VerifyTitle("*BBC*");

            Assert.IsTrue(browser.GetTitle().Contains("BBC"));
        }

        [Test]
        public void ClickAndWaitTest()
        {
            browser
                .GoTo(homepageUrl)
                .Click("a:link")
                .WaitFor("img")
                .VerifyUrl("*bbc.co*");
            Assert.IsTrue(browser.GetTitle().Contains("BBC"));
        }


        [Test]
        public void ClickUsingXPathTest()
        {
            browser
                .GoTo(homepageUrl)
                .Click("//*[@id='orb-nav-links']/ul/li[1]/a");

            Assert.IsTrue(browser.GetTitle().Contains("BBC News"));
        }

        [Test]
        public void DoAndSubmitASearchTest()
        {
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            browser.Click(searchboxCss + " + input");
            Assert.IsTrue(browser.GetTitle().Contains("news"));
        }

        [Test]
        public void GetInputTest()
        {
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            var text = browser.Get(searchboxCss);
            Assert.AreEqual("news", text);
        }

        [Test]
        public void GetTest()
        {
            var text = browser
                .GoTo(homepageUrl)
                .Get("nav a");
            Assert.AreEqual("News", text);
        }

        [Test]
        public void GoToTest()
        {
            browser.GoTo(homepageUrl);
            Assert.IsTrue(browser.GetTitle().Contains("BBC"));
        }

        [Test]
        public void SubmitAndVerifyTest()
        {
            browser.GoTo("http://www.google.co.uk")
                .Set("input[name='q']", "hello world")
                .Submit()
                .WaitFor("div[role=main] a:link")
                .VerifyTitle("hello world*");
        }
    }
}