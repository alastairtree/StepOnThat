using System.Collections.Generic;

namespace StepOnThat.Infrastructure
{
    public interface IHasProperties : IList<Property>
    {
        string this[string name] { get; set; }
        void Override(IEnumerable<Property> properties);
        bool Contains(string name);
    }
}