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

        public IList<Step> Steps { get; set; }

        public override bool Equals(object obj)
        {
            var val = obj as Instructions;
            if (val == null) return false;

            return Steps.SequenceEqual(val.Steps);
        }

        public override int GetHashCode()
        {
            return Steps.GetHashCode();
        }
    }
}