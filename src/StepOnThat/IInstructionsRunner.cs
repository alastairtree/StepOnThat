using System.Collections.Generic;
using System.Threading.Tasks;

namespace StepOnThat
{
    public interface IInstructionsRunner
    {
        Task<bool> Run(Instructions instructions, IEnumerable<Property> propertiesToOverride = null,
            List<IStepResult> stepResults = null);
    }
}