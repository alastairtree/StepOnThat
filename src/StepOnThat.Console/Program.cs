using Autofac;
using StepOnThat.Infrastructure;
using System;
using System.Threading.Tasks;

namespace StepOnThat
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                var task = MainAsync(args);
                task.Wait();
                Console.WriteLine(task.Result.Message);
                return task.Result.ReturnCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }

        public static async Task<Execution> MainAsync(string[] args)
        {
            var resolver = new DependencyContainerBuilder();
            using (var scope = resolver.Container.BeginLifetimeScope())
            {
                var options = Options.TryParse(args);

                var result = new Execution { Options = options };
                var reader = scope.Resolve<InstructionsReaderWriter>();
                var runner = scope.Resolve<IInstructionsRunner>();

                if (options.IsValid)
                {
                    result.Instructions = reader.ReadFile(options.File);

                    if (result.Instructions.Properties.HasPromptProperties)
                    {
                        var form = new DynamicPromptForm(result.Instructions.Properties);
                        form.PromptForVariablesWithUI(() => runner.Run(result.Instructions, options.Properties));
                    }
                    else
                    {
                        await runner.Run(result);
                    }

                }
                return result;
            }
        }
    }
}
