using System;

namespace StepOnThat
{
    public class StepResult : IStepResult
    {
        public bool Success { get; private set; }
        public Exception Error { get; private set; }
        public TimeSpan Duration { get; set; }

        public static StepResult Succeeded()
        {
            return new StepResult {Success = true};
        }

        public static StepResult Failed(Exception error)
        {
            return new StepResult {Success = false, Error = error};
        }
    }
}