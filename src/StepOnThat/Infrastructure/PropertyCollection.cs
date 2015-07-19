using System;
using System.Collections.Generic;
using System.Linq;

namespace StepOnThat.Infrastructure
{
    internal class PropertyCollection : List<Property>, IHasProperties
    {
        public string this[string name]
        {
            get
            {
                var found = this.First(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
                return found.Value;
            }
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

        public void Override(IEnumerable<Property> properties)
        {
            if (properties == null) return;

            foreach (var property in properties)
            {
                this[property.Key] = property.Value;
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