namespace StepOnThat.Browser.Actions
{
    public class Address : BrowserAction, IMatch
    {
        public virtual string Match { get; set; }

        public override void Run(IWebBrowser browser)
        {
            browser.VerifyUrl(Match);
        }
    }
}