using System.Collections;
using CommandLine;
using CommandLine.Text;

namespace StepOnThat
{
    public class Options
    {
        public Options()
        {
            Properties = new string[0];
        }

        [Option('f', "File", HelpText = "Path of the JSON steps file to be processed", Required = true)]
        public string File { get; set; }

        [OptionArray('p', "Properties", HelpText = "Set values to properties that will override properties in the file. Syntax steponthat.exe -p key1=value1 key2=value2")]
        public string[] Properties { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}