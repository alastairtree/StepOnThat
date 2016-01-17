using StepOnThat.Steps;
using System.Collections.Generic;

namespace StepOnThat
{
    public class Execution
    {
        public Execution()
        {
            Success = false;
        }

        public Options Options { get; set; }
        public Instructions Instructions { get; set; }
        public List<IStepResult> StepResults { get; set; }
        public bool Success { get; set; }

        public int ReturnCode
        {
            get
            {
                return Success ? 0 : -1;
            }
        }

        public string Message
        {
            get
            {
                return string.Format("Result: {0}", Success ? "Success - you stepped on that!" : "Failure - doh you slipped up!");
            }
        }
    }
}