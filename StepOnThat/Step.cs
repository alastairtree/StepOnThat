using System.Diagnostics;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class Step
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var val = obj as Step;
            if (val == null) return false;

            return Type == val.Type;
        }

        public async Task<IStepResult> RunAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IStepResult result = await RunAsyncCore();

            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;

            return result;
        }

        protected virtual async Task<IStepResult> RunAsyncCore()
        {
            return await Task.Run(() => StepResult.Succeeded());
        }

        public override int GetHashCode()
        {
            return new
            {
                Type,
                Name
            }
                .GetHashCode();
        }
    }
}