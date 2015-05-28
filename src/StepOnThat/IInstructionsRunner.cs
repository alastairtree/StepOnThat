using System.Threading.Tasks;

namespace StepOnThat
{
    public interface IInstructionsRunner
    {
        Task<bool> Run(Instructions instructions);
    }
}