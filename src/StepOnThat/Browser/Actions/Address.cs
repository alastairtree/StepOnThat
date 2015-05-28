namespace StepOnThat.Browser.Actions
{
    public class Address : BrowserAction, IMatch
    {
        public override void Run(IWebBrowser browser)
        {
            browser.VerifyUrl(Match);
        }

        public virtual string Match { get; set; }
    }
}