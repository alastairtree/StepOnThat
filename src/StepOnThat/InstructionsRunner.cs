using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StepOnThat.Infrastructure;

namespace StepOnThat
{
    public class InstructionsRunner : IInstructionsRunner
    {
        private IExecuteSteps stepRunner;

        public InstructionsRunner(IExecuteSteps stepRunner)
        {
            this.stepRunner = stepRunner;
        }

        public async Task<bool> Run(Instructions instructions)
        {
            if (instructions == null || instructions.Steps.IsNullOrEmpty())
                return true;

            var results = new List<IStepResult>();

            foreach (var step in instructions.Steps)
            {
                var result = await stepRunner.ExecuteStep(step);
                results.Add(result);

                if (!result.Success)
                    return false;
            }

            return results.All(x => x.Success);
        }
    }
}