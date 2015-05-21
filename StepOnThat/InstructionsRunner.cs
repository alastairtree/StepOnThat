using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class InstructionsRunner
    {
        public async Task<bool> Run(Instructions instructions)
        {
            if (instructions == null || instructions.Steps.IsNullOrEmpty())
                return true;

            var results = new List<IStepResult>();

            foreach (var step in instructions.Steps)
            {
                var result = await step.RunAsync();
                results.Add(result);

                if (!result.Success)
                    return false;
            }

            return results.All(x => x.Success);
        }
    }
}