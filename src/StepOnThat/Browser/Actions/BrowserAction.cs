namespace StepOnThat.Browser.Actions
{
    public abstract class BrowserAction
    {
        public string Action
        {
            get { return GetType().Name; }
        }

        public abstract void Run(IWebBrowser browser);
    }
}