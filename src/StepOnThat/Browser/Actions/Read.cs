namespace StepOnThat.Steps.Browser.Actions
{
    public class Read : Interaction
    {
        public virtual string Value { get; set; }

        public override void Run(IWebBrowser browser)
        {
            Value = browser.Get(Target);
        }
    }
}