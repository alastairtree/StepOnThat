using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class InstructionsRunner : IInstructionsRunner
    {
        private readonly IExecuteSteps stepRunner;

        public InstructionsRunner(IExecuteSteps stepRunner)
        {
            this.stepRunner = stepRunner;
        }

        public async Task<bool> Run(Instructions instructions, IEnumerable<Property> propertiesToOverride = null, List<IStepResult> results = null)
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

        private void ApplyProperties(Instructions instructions, IEnumerable<Property> propertiesToOverride)
        {
            if (propertiesToOverride == null) return;

            foreach (var property in propertiesToOverride)
            {
                instructions.Properties[property.Key] = property.Value;
            }
        }
    }
}