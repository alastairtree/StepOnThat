using NUnit.Framework;

namespace StepOnThat.Browser.Tests
{
    [TestFixture]
    [Category("Browser")]
    public class BrowserTests
    {
        private string homepageUrl = "http://www.bbc.co.uk";
        private string searchboxCss = "input[type=text][name=q]";
        [Test]
        public void GoToTest()
        {
            var browser = Browser.Current;
            browser.GoTo(homepageUrl);
            Assert.IsTrue(browser.Title().Contains("BBC"));
        }

        [Test]
        public void ClickAndWaitTest()
        {
            Browser.Current
                .GoTo(homepageUrl)
                .Click("a:link")
                .WaitFor("img");
            Assert.IsTrue(Browser.Current.Title().Contains("BBC"));
        }

        
        [Test]
        public void ClickUsingXPathTest()
        {
            Browser.Current
                .GoTo(homepageUrl)
                .Click("//*[@id='orb-nav-links']/ul/li[1]/a");

            Assert.IsTrue(Browser.Current.Title().Contains("BBC News"));
        }

        [Test]
        public void DoAndSubmitASearchTest()
        {
            var browser = Browser.Current;
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            browser.Click(searchboxCss + " + input");
            Assert.IsTrue(browser.Title().Contains("news"));
        }

        [Test]
        public void GetTest()
        {
            var browser = Browser.Current;
            var text = browser
                .GoTo(homepageUrl)
                .Get("nav a");
            Assert.AreEqual("News", text);
        }

        [Test]
        public void GetInputTest()
        {
            var browser = Browser.Current;
            browser.GoTo(homepageUrl);
            browser.Set(searchboxCss, "news");
            var text = browser.Get(searchboxCss);
            Assert.AreEqual("news", text);
        }

        [Test]
        public void BackAndForwardTest()
        {
            Browser.Current
                .GoTo(homepageUrl)
                .Click("a:link")
                .Back()
                .Forward();
            Assert.IsTrue(Browser.Current.Title().Contains("BBC"));
        }

        [TestFixtureTearDown]
        public void AfterAlltests()
        {
            Browser.Current.Close();
        }
    }
}