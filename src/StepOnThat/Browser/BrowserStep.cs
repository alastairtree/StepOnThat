using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StepOnThat.Browser;
using StepOnThat.Browser.Actions;

namespace StepOnThat.Tests.Browser
{
    public class BrowserStep : Step
    {
        private readonly IWebBrowser browser;

        public BrowserStep(IWebBrowser browser, string url = null)
        {
            Url = url;
            this.browser = browser;
            Steps = new List<BrowserAction>();
        }

        public virtual string Url { get; set; }

        public ICollection<BrowserAction> Steps { get; private set; }

        public override async Task<IStepResult> RunAsync()
        {
            if (browser == null)
                throw new NullReferenceException("Browser step is missing its IWebBrowser dependency browser!");

            return await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(Url))
                    browser.GoTo(Url);

                foreach (var browserActionStep in Steps)
                    browserActionStep.Run(browser);

                browser.Close();

                return StepResult.Succeeded();
            });
        }
    }
}