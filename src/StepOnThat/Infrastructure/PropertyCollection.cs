using System.Collections.Generic;
using System.Linq;

namespace StepOnThat.Infrastructure
{
    internal class PropertyCollection : List<Property>, IHasProperties
    {
        public string this[string name]
        {
            get { return this.Single(x => x.Key == name).Value; }
            set
            {
                if (this.Any(x => x.Key == name))
                {
                    Remove(this.Single(x => x.Key == name));
                    Add(new Property(name, value));
                }
                else
                {
                    Add(new Property(name, value));
                }
            }
        }

        public Property Add(string key, string value)
        {
            var prop = new Property(key, value);
            Add(prop);
            return prop;
        }
    }
}