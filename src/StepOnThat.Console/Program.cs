using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using StepOnThat.Infrastructure;

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

        public static async Task<ExecutionResult> MainAsync(string[] args)
        {
            var result = new ExecutionResult();

            if (Parser.Default.ParseArguments(args, result.Options))
            {
                var overrideProperties = result.Options.Properties.Select(PropertyParser.Get);

                if (File.Exists(result.Options.File))
                {
                    var resolver = new DependencyResolver();
                    using (var scope = resolver.Container.BeginLifetimeScope())
                    {
                        var reader = new InstructionsReaderWriter(scope);

                        var instructions = reader.ReadFile(result.Options.File);
                        result.Instructions = instructions;

                        var runner = scope.Resolve<IInstructionsRunner>();
                        result.InstructionsRunner = runner;

                        if (instructions.Properties.HasPromptProperties)
                        {
                            var form = new DynamicPromptForm(instructions.Properties);
                            form.PromptForVariablesWithUI(() => RunSteps(result, overrideProperties));
                        }
                        else
                        {
                            await RunSteps(result, overrideProperties);
                        }
                    }

                    result.Message = string.Format("Result: {0}",
                        result.Success ? "Success - you stepped on that!" : "Failure - doh you slipped up!");
                    ;
                }
            }

            return result;
        }

        private static async Task RunSteps(ExecutionResult result, IEnumerable<Property> overrideProperties)
        {
            result.Success =
                await
                    result.InstructionsRunner.Run(result.Instructions, overrideProperties,
                        result.StepResults);
        }
    }
}
