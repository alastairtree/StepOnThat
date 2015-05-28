using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepOnThat.Infrastructure
{
    class VariableStore : IVariables
    {
        public VariableStore()
        {
            this.Items = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Items { get; private set; }

        public string this[string name]
        {
            get { return Items[name]; }
            set { Items[name] = value; }
        }
    }
}
