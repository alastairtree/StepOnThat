using System.Threading.Tasks;

namespace StepOnThat
{
    public interface IExecuteSteps
    {
        Task<IStepResult> ExecuteStep(Step step);
    }
}