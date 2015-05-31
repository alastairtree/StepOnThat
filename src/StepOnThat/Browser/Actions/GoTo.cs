namespace StepOnThat.Browser.Actions
{
    public class GoTo : BrowserAction
    {
        public virtual string Url { get; set; }

        public override void Run(IWebBrowser browser)
        {
            browser.GoTo(Url);
        }
    }
}