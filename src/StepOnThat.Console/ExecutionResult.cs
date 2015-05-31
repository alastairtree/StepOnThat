using System.Collections.Generic;

namespace StepOnThat
{
    public class ExecutionResult
    {
        public ExecutionResult()
        {
            Options = new Options();
            ReturnCode = -1;
            Success = false;
        }

        public Options Options { get; set; }
        public Instructions Instructions { get; set; }
        public IInstructionsRunner InstructionsRunner { get; set; }
        public List<IStepResult> StepResults { get; set; }
        public bool Success { get; set; }
        public int ReturnCode { get; set; }
        public string Message { get; set; }
    }
}