using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StepOnThat.Steps;

namespace StepOnThat
{
    public class InstructionsRunner : IInstructionsRunner
    {
        private readonly IExecuteSteps stepRunner;

        public InstructionsRunner(IExecuteSteps stepRunner)
        {
            this.stepRunner = stepRunner;
        }

        public async Task<bool> Run(Instructions instructions, IEnumerable<Property> propertiesToOverride = null,
            List<IStepResult> results = null)
        {
            if (instructions == null || instructions.Steps.IsNullOrEmpty())
                return true;

            ApplyProperties(instructions, propertiesToOverride);

            results = results ?? new List<IStepResult>();

            foreach (var step in instructions.Steps)
            {
                var result = await stepRunner.ExecuteStep(step);
                results.Add(result);

                if (!result.Success)
                    return false;
            }

            return results.All(x => x.Success);
        }

        public async Task<Execution> Run(Execution execution)
        {
            execution.Success = await Run(execution.Instructions, execution.Options.Properties, execution.StepResults);
            return execution;
        }

        private void ApplyProperties(Instructions instructions, IEnumerable<Property> propertiesToOverride)
        {
            instructions.Properties.Override(propertiesToOverride);
        }
    }
}