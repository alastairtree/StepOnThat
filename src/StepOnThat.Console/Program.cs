using System;
using System.Collections.Generic;
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

                        var instructions = reader.ReadFile(result.Options.File);
                        result.Instructions = instructions;

                        var runner = scope.Resolve<IInstructionsRunner>();
                        result.InstructionsRunner = runner;

                        if (instructions.Properties.HasPromptProperties)
                        {
                            await PromptForVariablesWithUI(result, overrideProperties);
                        }
                        else
                        {
                            await RunSteps(result, overrideProperties);
                        }
                    }

                    result.Message = string.Format("Result: {0}",
                        result.Success ? "Success - you stepped on that!" : "Failure - doh you slipped up!");

                    result.ReturnCode = result.Success ? 0 : -1;
                }
            }

            return result;
        }

        private static async Task PromptForVariablesWithUI(ExecutionResult result,
            IEnumerable<Property> overrideProperties)
        {
            Application.EnableVisualStyles();
            var f = new Form();
            var formWidth = 400;
            var controlHeight = 32;
            var spaceBetweenControls = 12;
            f.Padding = new Padding(spaceBetweenControls);
            f.BackColor = Color.DimGray;
            f.Width = formWidth + spaceBetweenControls;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.None;
            var prompts = result.Instructions.Properties.Count;
            f.Height = spaceBetweenControls* 2 + (controlHeight*(prompts + 1));
            var font = new Font("Tahoma", 11f, FontStyle.Regular);
            for (int index = 0; index < prompts; index++)
            {
                var property = result.Instructions.Properties[index];

                var verticalPosition = spaceBetweenControls + index*(controlHeight + spaceBetweenControls);
                f.Controls.Add(new Label
                {
                    Text = property.Key.Replace(":prompt", ""),
                    Size = new Size((formWidth/3), controlHeight),
                    TextAlign = ContentAlignment.MiddleRight,
                    Location = new Point(spaceBetweenControls, verticalPosition - 5),
                    ForeColor = Color.White,
                    Padding = new Padding(0,0,spaceBetweenControls, 0),
                    Font = font
                });
                f.Controls.Add(new TextBox
                {
                    Name = property.Key,
                    Size = new Size((formWidth/3)*2 - spaceBetweenControls, controlHeight),
                    Location = new Point(formWidth/3 + spaceBetweenControls, verticalPosition),
                    Font = font
                });
            }

            var button = new Button
            {
                Text = "Submit",
                Size = new Size(formWidth/4, controlHeight),
                Location = new Point((3*formWidth)/4, prompts + (controlHeight + spaceBetweenControls)),
                BackColor = Color.White,
                ForeColor = Color.DodgerBlue,
                Font = font

            };
            button.Click += async (obj, evt) =>
            {
                f.Close();
                await RunSteps(result, overrideProperties);
            };
            f.Controls.Add(button);

            var closeButton = new Button
            {
                Text = "Exit",
                Size = new Size(formWidth/4, controlHeight),
                Location = new Point( (formWidth / 4) * 2, prompts + 1 * (controlHeight + spaceBetweenControls)),
                BackColor = Color.White,
                ForeColor = Color.Gray,
                Font = font

            };
            closeButton.Click += (obj, evt) =>
            {
                f.Close();
                result.ReturnCode = 0;
            };
            f.Controls.Add(closeButton);

            f.ShowDialog();
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
