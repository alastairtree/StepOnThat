using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StepOnThat.Browser
{
    public class Browser
    {
        private static Browser currentBrowser;

        public static int DefaultWaitInSeconds = 5;

        private IWebDriver driver;

        /// <summary>
        /// Defaults to Chrome
        /// </summary>
        public Browser() 
            : this(new ChromeDriver(new ChromeOptions()))
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(DefaultWaitInSeconds));
        }

        public Browser(IWebDriver driver)
        {
            this.Driver = driver;
        }

        /// <summary>
        /// Get the current (or a new) browser. Chrome by default.
        /// </summary>
        public static Browser Current
        {
            get { return currentBrowser ?? (currentBrowser = new Browser()); }
            set
            {
                if (currentBrowser != null)
                {
                    currentBrowser.Close();
                }
                currentBrowser = value;
            }
        }

        public IWebDriver Driver
        {
            get { return driver; }
            set
            {
                Close(); //clean up before swapping driver
                driver = value;
            }
        }

        public Browser GoTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
            return this;
        }

        public Browser Back()
        {
            Driver.Navigate().Back();
            return this;
        }

        public Browser Forward()
        {
            Driver.Navigate().Forward();
            return this;
        }

        public Browser Click(string cssOrXpathSelector)
        {
            var elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.Click();
            return this;
        }

        private static By GetSelector(string cssOrXpathSelector)
        {
            if (cssOrXpathSelector.Contains("/") || cssOrXpathSelector.Contains("::"))
                return By.XPath(cssOrXpathSelector);

            return By.CssSelector(cssOrXpathSelector);
        }

        public Browser Set(string cssOrXpathSelector, string value)
        {
            var elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

            elem.SendKeys(value);
            return this;
        }

        public string Get(string cssOrXpathSelector)
        {
            var elem = Driver.FindElement(GetSelector(cssOrXpathSelector));

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

        public Browser WaitFor(string cssOrXpathSelector)
        {
            WaitFor(cssOrXpathSelector, DefaultWaitInSeconds);
            return this;
        }

        public Browser WaitFor(string cssOrXpathSelector, int seconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
            var elem = wait.Until(d => d.FindElement(GetSelector(cssOrXpathSelector)));
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