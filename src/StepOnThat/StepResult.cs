using System;

namespace StepOnThat.Steps
{
    public class StepResult : IStepResult
    {
        public virtual bool Success { get; private set; }
        public virtual Exception Error { get; private set; }
        public virtual TimeSpan Duration { get; set; }

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