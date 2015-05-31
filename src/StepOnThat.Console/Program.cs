using System;
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

                        result.Instructions = reader.ReadFile(result.Options.File);

                        result.InstructionsRunner = scope.Resolve<IInstructionsRunner>();

                        result.Success =
                            await
                                result.InstructionsRunner.Run(result.Instructions, overrideProperties,
                                    result.StepResults);
                    }

                    result.Message = string.Format("Result: {0}",
                        result.Success ? "Success - you stepped on that!" : "Failure - doh you slipped up!");

                    result.ReturnCode = result.Success ? 0 : -1;
                }
            }

            return result;
        }
    }
}