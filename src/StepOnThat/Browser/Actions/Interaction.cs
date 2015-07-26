namespace StepOnThat.Steps.Browser.Actions
{
    public abstract class Interaction : BrowserAction
    {
        /// <summary>
        ///     A CSS or XPath query selector
        /// </summary>
        public virtual string Target { get; set; }
    }
}