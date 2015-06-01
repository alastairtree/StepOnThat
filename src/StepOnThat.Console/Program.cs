using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

                        result.Instructions = reader.ReadFile(result.Options.File);

                        result.InstructionsRunner = scope.Resolve<IInstructionsRunner>();

                        if (result.Instructions.Properties.Any(p => p.Key.EndsWith(":prompt")))
                        {
                            Application.EnableVisualStyles();
                            var f = new Form();
                            var s = new Size(200, 45);
                            for (int index = 0; index < result.Instructions.Properties.Count; index++)
                            {
                                var property = result.Instructions.Properties[index];
                                f.Controls.Add(new Label
                                {
                                    Text = property.Key.Replace(":prompt", ""),
                                    Size = s,
                                    Location = new Point(0, index * 60)
                                });
                                f.Controls.Add(new TextBox { Name = property.Key, Size = s, Location = new Point(0, index * 60 + 30) });
                                
                            }
                            var button = new Button { Text = "Submit", Size = s, Location = new Point(0, result.Instructions.Properties.Count * 60) };
                            f.Controls.Add(button);
                            button.Click += async (obj, evt) =>
                            {
                                f.Close();
                                result.Success =
                                    await
                                        result.InstructionsRunner.Run(result.Instructions, overrideProperties,
                                            result.StepResults);
                            };
                            f.ShowDialog();
                        }
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
