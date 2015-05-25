namespace StepOnThat.Browser.Actions
{
    public class Title : BrowserAction, IMatch
    {
        public override void Run(IWebBrowser browser)
        {
            browser.VerifyTitle(Match);
        }

        public string Match { get; set; }
    }
}