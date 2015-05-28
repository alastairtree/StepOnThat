namespace StepOnThat.Browser.Actions
{
    public class Set : Interaction
    {
        public virtual string Value { get; set; }

        public override void Run(IWebBrowser browser)
        {
            browser.Set(Target, Value);
        }
    }
}