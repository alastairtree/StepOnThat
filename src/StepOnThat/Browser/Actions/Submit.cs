using System;

namespace StepOnThat.Steps.Browser.Actions
{
    public class Submit : Interaction
    {
        public override void Run(IWebBrowser browser)
        {
            if (String.IsNullOrEmpty(Target))
            {
                browser.Submit();
            }
            else
            {
                browser.Submit(Target);
            }
        }
    }
}