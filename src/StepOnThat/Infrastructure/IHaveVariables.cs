using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepOnThat.Infrastructure
{
    public interface IVariables
    {
        IDictionary<string,string> Items { get; }
        string this[string name] { get; set; }
    }
}
