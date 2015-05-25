namespace StepOnThat.Browser.Actions
{
    public class Address : BrowserAction, IMatch
    {
        public override void Run(IWebBrowser browser)
        {
            browser.VerifyUrl(Match);
        }

        public string Match { get; set; }
    }
}