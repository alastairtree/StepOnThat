using System.Collections.Generic;
using System.Linq;

namespace StepOnThat
{
    public class Instructions
    {
        public Instructions() : this(new List<Step>())
        {
        }

        public Instructions(List<Step> steps)
        {
            Steps = steps;
        }

        public Instructions(IEnumerable<Step> steps)
        {
            Steps = new List<Step>(steps);
        }

        public IList<Step> Steps { get; set; }

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