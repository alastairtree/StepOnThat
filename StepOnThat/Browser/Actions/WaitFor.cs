namespace StepOnThat.Browser.Actions
{
    public class WaitFor : Interaction
    {
        public override void Run(IWebBrowser browser)
        {
            browser.WaitFor(Target);
        }
    }
}