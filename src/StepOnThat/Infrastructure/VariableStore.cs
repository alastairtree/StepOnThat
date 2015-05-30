using System.Collections.Generic;

namespace StepOnThat.Infrastructure
{
    internal class VariableStore : IVariables
    {
        public VariableStore()
        {
            Items = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Items { get; private set; }

        public string this[string name]
        {
            get { return Items[name]; }
            set { Items[name] = value; }
        }
    }
}