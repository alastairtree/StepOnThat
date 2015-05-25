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
        WebBrowser WaitFor(string cssOrXpathSelector);
        WebBrowser WaitFor(string cssOrXpathSelector, int seconds);
        void Close();
    }
}