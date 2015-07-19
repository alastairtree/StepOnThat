using System.Threading.Tasks;
using StepOnThat.Steps;

namespace StepOnThat
{
    public interface IExecuteSteps
    {
        Task<IStepResult> ExecuteStep(Step step);
    }
}