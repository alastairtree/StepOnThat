using System.Collections.Generic;

namespace StepOnThat.Infrastructure
{
    public interface IVariables
    {
        IDictionary<string, string> Items { get; }
        string this[string name] { get; set; }
    }
}