namespace StepOnThat.Steps.Browser.Actions
{
    public class Title : BrowserAction, IMatch
    {
        public virtual string Match { get; set; }

        public override void Run(IWebBrowser browser)
        {
            browser.VerifyTitle(Match);
        }
    }
}