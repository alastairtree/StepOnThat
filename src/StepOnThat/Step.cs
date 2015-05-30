using System.Threading.Tasks;

namespace StepOnThat
{
    public class Step
    {
        public string Type
        {
            get { return GetType().Name.Replace("Proxy",""); }
        }

        public virtual string Name { get; set; }

        public override bool Equals(object obj)
        {
            var val = obj as Step;
            if (val == null) return false;

            return Type == val.Type &&
                   Name == val.Name;
        }

        public virtual async Task<IStepResult> RunAsync()
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