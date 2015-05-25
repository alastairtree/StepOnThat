namespace StepOnThat.Browser.Actions
{
    public class Verify : Interaction, IMatch
    {
        public string Match { get; set; }

        public override void Run(IWebBrowser browser)
        {
            if (Match.IsNullOrEmpty())
            {
                browser.VerifyElement(Target);
            }
            else
            {
                browser.VerifyText(Target, Match);
            }
        }
    }
}