using System.Threading.Tasks;
using StepOnThat.Steps;

namespace StepOnThat
{
    public class Echo : Step
    {
        private readonly IOutput output;

        public Echo(IOutput output)
        {
            this.output = output;
            Text = string.Empty;
        }

        public virtual string Text { get; set; }

        public override bool Equals(object obj)
        {
            var echo = obj as Echo;
            return base.Equals(obj) &&
                   echo != null &&
                   echo.Text.Equals(Text);
        }

        public override Task<IStepResult> RunAsync()
        {
            if (!string.IsNullOrEmpty(Text))
                output.Write(Text);

            return base.RunAsync();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()*Text.GetHashCode();
        }
    }
}