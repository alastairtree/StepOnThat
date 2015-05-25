using NUnit.Framework;

namespace StepOnThat.Browser.Tests
{
    [TestFixture]
    [Category("WebBrowser")]
    public class WebBrowserTests
    {
        private string homepageUrl = "http://www.bbc.co.uk";
        private string searchboxCss = "input[type=text][name=q]";
        [Test]
        public void GoToTest()
        {
            var browser = WebBrowser.Current;
            browser.GoTo(homepageUrl);
            Assert.IsTrue(browser.Title().Contains("BBC"));
        }

        [Test]
        public void ClickAndWaitTest()
        {
            WebBrowser.Current
                .GoTo(homepageUrl)
                .Click("a:link")
                .WaitFor("img");
            Assert.IsTrue(WebBrowser.Current.Title().Contains("BBC"));
        }

        
        [Test]
        public void ClickUsingXPathTest()
        {
            WebBrowser.Current
                .GoTo(homepageUrl)
                .Click("//*[@id='orb-nav-links']/ul/li[1]/a");

            Assert.IsTrue(WebBrowser.Current.Title().Contains("BBC News"));
        }

        [Test]
        public void DoAndSubmitASearchTest()
        {
            var browser = WebBrowser.Current;
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            browser.Click(searchboxCss + " + input");
            Assert.IsTrue(browser.Title().Contains("news"));
        }

        [Test]
        public void GetTest()
        {
            var browser = WebBrowser.Current;
            var text = browser
                .GoTo(homepageUrl)
                .Get("nav a");
            Assert.AreEqual("News", text);
        }

        [Test]
        public void GetInputTest()
        {
            var browser = WebBrowser.Current;
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            var text = browser.Get(searchboxCss);
            Assert.AreEqual("news", text);
        }

        [Test]
        public void BackAndForwardTest()
        {
            WebBrowser.Current
                .GoTo(homepageUrl)
                .Click("a:link")
                .Back()
                .Forward();
            Assert.IsTrue(WebBrowser.Current.Title().Contains("BBC"));
        }

        [TestFixtureTearDown]
        public void AfterAlltests()
        {
            WebBrowser.Current.Close();
        }
    }
}