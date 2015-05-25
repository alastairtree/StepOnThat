namespace StepOnThat.Browser.Actions
{
    public abstract class Back : BrowserAction
    {
        public override void Run(IWebBrowser browser)
        {
            browser.Back();
        }
    }
}