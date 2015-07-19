using System;
using System.Threading.Tasks;
using Autofac;
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
                    await runner.Run(result);
                }
                return result;
            }
        }
    }
}