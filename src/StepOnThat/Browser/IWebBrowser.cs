namespace StepOnThat.Browser
{
    public interface IWebBrowser
    {
        WebBrowser GoTo(string url);
        WebBrowser Back();
        WebBrowser Forward();
        WebBrowser Click(string cssOrXpathSelector);
        WebBrowser Set(string cssOrXpathSelector, string value);
        string Get(string cssOrXpathSelector);
        string GetTitle();
        WebBrowser WaitFor(string cssOrXpathSelector);
        WebBrowser WaitFor(string cssOrXpathSelector, int seconds);
        WebBrowser Submit();
        WebBrowser Submit(string cssOrXpathSelector);
        WebBrowser VerifyElement(string cssOrXPathSelector);
        WebBrowser VerifyText(string cssOrXPathSelector, string wildcardText);
        WebBrowser VerifyTitle(string wildcardText, int? seconds = null);
        void Close();
    }
}