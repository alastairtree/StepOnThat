using System;

namespace StepOnThat
{
    public interface IStepResult
    {
        bool Success { get; }
        Exception Error { get; }
        TimeSpan Duration { get; set; }
    }
}