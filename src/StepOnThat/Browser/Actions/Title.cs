namespace StepOnThat.Browser.Actions
{
    public class Title : BrowserAction, IMatch
    {
        public override void Run(IWebBrowser browser)
        {
            browser.VerifyTitle(Match);
        }

        public virtual string Match { get; set; }
    }
}