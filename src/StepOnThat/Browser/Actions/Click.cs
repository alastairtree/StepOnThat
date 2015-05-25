namespace StepOnThat.Browser.Actions
{
    public class Click : Interaction
    {
        public override void Run(IWebBrowser browser)
        {
            browser.Click(Target);
        }
    }
}