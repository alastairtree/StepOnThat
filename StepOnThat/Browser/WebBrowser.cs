using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StepOnThat.Browser
{
    public class WebBrowser : IWebBrowser
    {
        private static WebBrowser currentWebBrowser;

        private static int defaultWaitInSeconds = 5;

        private IWebDriver driver;
        private Func<IWebDriver> driverFactory;

        /// <summary>
        ///     Defaults to Chrome
        /// </summary>
        public WebBrowser()
            : this(() => new ChromeDriver(new ChromeOptions()))
        {
        }

        public WebBrowser(Func<IWebDriver> driverFactory)
        {
            this.driverFactory = driverFactory;
        }

        /// <summary>
        /// Get the current (or a new) browser. Chrome by default.
        /// </summary>
        public static WebBrowser Current
        {
            get { return currentWebBrowser ?? (currentWebBrowser = new WebBrowser()); }
            set
            {
                if (currentWebBrowser != null)
                {
                    currentWebBrowser.Close();
                }
                currentWebBrowser = value;
            }
        }

        public IWebDriver Driver
        {
            get
            {
                if (driver == null)
                {
                    driver = driverFactory();
                    Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(defaultWaitInSeconds));
                }
                return driver;
            }
            set
            {
                Close(); //clean up before swapping driver
                driver = value;
            }
        }

        public static int DefaultWaitInSeconds
        {
            get { return defaultWaitInSeconds; }
            set { defaultWaitInSeconds = value; }
        }

        public WebBrowser GoTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
            return this;
        }

        public WebBrowser Back()
        {
            Driver.Navigate().Back();
            return this;
        }

        public WebBrowser Forward()
        {
            Driver.Navigate().Forward();
            return this;
        }

        public WebBrowser Click(string cssOrXpathSelector)
        {
            IWebElement elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.Click();
            return this;
        }

        private static By GetSelector(string cssOrXpathSelector)
        {
            if (cssOrXpathSelector.Contains("/") || cssOrXpathSelector.Contains("::"))
                return By.XPath(cssOrXpathSelector);

            return By.CssSelector(cssOrXpathSelector);
        }

        public WebBrowser Set(string cssOrXpathSelector, string value)
        {
            IWebElement elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.SendKeys(value);
            return this;
        }

        public string Get(string cssOrXpathSelector)
        {
            IWebElement elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

            if (elem.TagName.ToLowerInvariant() == "input" && elem.GetAttribute("type") == "text")
                return elem.GetAttribute("value");

            if (elem.TagName.ToLowerInvariant() == "select")
                return elem.GetAttribute("value");

            return elem.Text;
        }

        public string Title()
        {
            return Driver.Title;
        }

        public WebBrowser WaitFor(string cssOrXpathSelector)
        {
            WaitFor(cssOrXpathSelector, defaultWaitInSeconds);
            return this;
        }

        public WebBrowser WaitFor(string cssOrXpathSelector, int seconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
            IWebElement elem = wait.Until(d => d.FindElement(GetSelector(cssOrXpathSelector)));
            return this;
        }

        public void Close()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Dispose();
                driver = null;
            }
        }
    }
}