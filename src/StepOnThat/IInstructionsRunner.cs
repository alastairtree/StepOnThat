using System.Collections.Generic;
using System.Threading.Tasks;
using StepOnThat.Steps;

namespace StepOnThat
{
    public interface IInstructionsRunner
    {
        Task<bool> Run(Instructions instructions, IEnumerable<Property> propertiesToOverride = null,
            List<IStepResult> stepResults = null);

        Task<Execution> Run(Execution execution);
    }
}