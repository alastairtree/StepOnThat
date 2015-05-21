using CommandLine;
using CommandLine.Text;

namespace StepOnThat
{
    public class Options
    {
        [Option('f', "File", HelpText = "Path of the JSON steps file to be processed", Required = true)]
        public string File { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}