namespace StepOnThat.Browser.Actions
{
    public abstract class Forward : BrowserAction
    {
        public override void Run(IWebBrowser browser)
        {
            browser.Forward();
        }
    }
}