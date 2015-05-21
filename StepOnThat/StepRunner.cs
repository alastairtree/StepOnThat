using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class StepRunner : IExecuteSteps
    {
        public async Task<IStepResult> ExecuteStep(Step step)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IStepResult result = await step.RunAsync();

            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;

            return result;
        }
    }
}
