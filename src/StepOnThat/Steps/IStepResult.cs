using System;

namespace StepOnThat.Steps
{
    public interface IStepResult
    {
        bool Success { get; }
        Exception Error { get; }
        TimeSpan Duration { get; set; }
    }
}