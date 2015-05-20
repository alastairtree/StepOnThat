using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class Step
    {
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            var val = obj as Step;
            if (val == null) return false;

            return Type == val.Type;
        }

        public override int GetHashCode()
        {
            return new
            {
                Type,

            }.GetHashCode();
        }
    }
}
