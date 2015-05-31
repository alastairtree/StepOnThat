using System.Collections.Generic;
using System.Linq;
using StepOnThat.Infrastructure;

namespace StepOnThat
{
    public class Instructions
    {
        public Instructions(IEnumerable<Step> steps)
            : this(new PropertyCollection(), steps)
        {
        }

        public Instructions(IHasProperties props, IEnumerable<Step> steps = null)
        {
            Steps = (steps == null ? new List<Step>() : new List<Step>(steps));
            Properties = props;
        }

        public IList<Step> Steps { get; set; }

        public IHasProperties Properties { get; private set; }

        public override bool Equals(object obj)
        {
            var val = obj as Instructions;
            if (val == null) return false;

            return Steps.SequenceEqual(val.Steps);
        }

        public override int GetHashCode()
        {
            int hash = 27;
            foreach (var step in Steps)
            {
                hash = (13*hash) + step.GetHashCode();
            }
            return hash;
        }
    }
}