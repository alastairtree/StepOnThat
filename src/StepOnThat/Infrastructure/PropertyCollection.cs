using System.Collections;
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
                    var prop = this.Single(x => x.Key == name);
                    prop.Value = value;
                }
                else
                {
                    this.Add(new Property() {Key = name, Value = value});
                }

            }
        }
    }
}