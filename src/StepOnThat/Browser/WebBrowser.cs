using System;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace StepOnThat.Browser
{
    public class WebBrowser : IWebBrowser
    {
        private static WebBrowser currentWebBrowser;

        private static int defaultWaitInSeconds = 5;
        private IWebElement currentElement;

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
        ///     Get the current (or a new) browser. Chrome by default.
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
            currentElement = null;
            return this;
        }

        public WebBrowser Back()
        {
            Driver.Navigate().Back();
            currentElement = null;
            return this;
        }

        public WebBrowser Forward()
        {
            Driver.Navigate().Forward();
            currentElement = null;
            return this;
        }

        public WebBrowser Click(string cssOrXpathSelector)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXpathSelector));

            currentElement.Click();
            return this;
        }

        public WebBrowser Set(string cssOrXpathSelector, string value)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXpathSelector));

            currentElement.SendKeys(value);
            return this;
        }

        public string Get(string cssOrXpathSelector)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXpathSelector));

            if (currentElement.TagName.ToLowerInvariant() == "input" && currentElement.GetAttribute("type") == "text")
                return currentElement.GetAttribute("value");

            if (currentElement.TagName.ToLowerInvariant() == "select")
                return currentElement.GetAttribute("value");

            return currentElement.Text;
        }

        public WebBrowser WaitFor(string cssOrXpathSelector)
        {
            WaitFor(cssOrXpathSelector, defaultWaitInSeconds);
            return this;
        }

        public WebBrowser WaitFor(string cssOrXpathSelector, int seconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
            currentElement = wait.Until(d => d.FindElement(GetSelector(cssOrXpathSelector)));
            return this;
        }

        public WebBrowser VerifyUrl(string urlWildcard, int? seconds = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds ?? defaultWaitInSeconds));
            var lastUrl = "";
            try
            {
                wait.Until(d => Regex.IsMatch((lastUrl = d.Url), WildcardToRegex(urlWildcard)));
            }
            catch (WebDriverTimeoutException)
            {
                throw new ApplicationException(string.Format("Could not match url '{0}' to wildcard '{1}'", lastUrl,
                    urlWildcard));
            }

            return this;
        }

        public void Close()
        {
            if (driver != null)
            {
                currentElement = null;
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }

        public string GetTitle()
        {
            return Driver.Title;
        }

        public WebBrowser Submit()
        {
            //if we had an input element in the previous step, lets use it to submit with an enter press
            if (currentElement != null && currentElement.TagName == "input")
            {
                currentElement.SendKeys(Keys.Enter);
                currentElement = null; //dont retain the element after submission
                return this;
            }

            //otherwise try and guess the button to submit
            const string submitSelector = "input[type=submit], button[type=submit]";
            var submits = Driver.FindElements(By.CssSelector(submitSelector));

            if (submits.Count == 1)
            {
                submits.Single().Click();
                return this;
            }

            //try and guess the form to submit
            submits = Driver.FindElements(By.TagName("form"));

            if (submits.Count == 1)
            {
                submits.Single().Submit();
                return this;
            }

            throw new ApplicationException("Cannot find the form to submit automatically. Specify one explicitly.");
        }

        public WebBrowser Submit(string cssOrXpathSelector)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXpathSelector));
            currentElement.Submit();
            currentElement = null;
            return this;
        }

        public WebBrowser VerifyElement(string cssOrXPathSelector)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXPathSelector));
            return this;
        }

        public WebBrowser VerifyText(string cssOrXPathSelector, string wildcardText)
        {
            currentElement = Driver.FindElement(GetSelector(cssOrXPathSelector));
            var elementText = currentElement.Text;

            if (!Regex.IsMatch(elementText, WildcardToRegex((wildcardText))))
                throw new ApplicationException(
                    string.Format("Found element using selector '{0}' but text '{1}' does not match wildcard '{2}'",
                        cssOrXPathSelector, elementText, wildcardText));

            return this;
        }

        public WebBrowser VerifyTitle(string titleWildcard, int? seconds = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds ?? defaultWaitInSeconds));
            var lastTitle = "";
            try
            {
                wait.Until(d => Regex.IsMatch((lastTitle = d.Title), WildcardToRegex(titleWildcard)));
            }
            catch (WebDriverTimeoutException)
            {
                throw new ApplicationException(string.Format("Could not match title '{0}' to wildcard '{1}'", lastTitle,
                    titleWildcard));
            }

            return this;
        }

        private static By GetSelector(string cssOrXpathSelector)
        {
            if (cssOrXpathSelector.Contains("/") || cssOrXpathSelector.Contains("::"))
                return By.XPath(cssOrXpathSelector);

            return By.CssSelector(cssOrXpathSelector);
        }

        private static string WildcardToRegex(string pattern)
        {
            return "(?i)^" + Regex.Escape(pattern).
                Replace("\\*", ".*").
                Replace("\\?", ".") + "$";
        }
    }
}