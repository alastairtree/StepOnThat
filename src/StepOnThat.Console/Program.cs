using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using StepOnThat.Infrastructure;

namespace StepOnThat
{
    public class Program
    {
        private static InstructionsRunner runner = new InstructionsRunner(new StepRunner());

        public static int Main(string[] args)
        {
            try
            {
                var task = MainAsync(args);
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return -1;
            }
        }

        private static async Task<int> MainAsync(string[] args)
        {
            var options = new Options();
            var returnCode = -1;
            bool result;

            if (Parser.Default.ParseArguments(args, options))
            {
                if (File.Exists(options.File))
                {
                    var resolver = new DependencyResolver();
                    using (var scope = resolver.Container.BeginLifetimeScope())
                    {
                        var reader = new InstructionsReaderWriter(scope);

                        var ins = reader.ReadFile(options.File);

                        result = await runner.Run(ins);
                    }


                    Console.WriteLine("Result: {0}",
                        result ? "Success - you stepped on that!" : "Failure - doh you slipped up!");

                    returnCode = result ? 0 : -1;
                }
            }

            return returnCode;
        }
    }
}