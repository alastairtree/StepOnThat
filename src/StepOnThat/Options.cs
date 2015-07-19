using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace StepOnThat
{
    public class Options
    {
        Options()
        {
            PropertyStrings = new string[0];
            IsValid = false;
        }

        [Option('f', "File", HelpText = "Path of the JSON steps file to be processed", Required = true)]
        public string File { get; set; }

        [OptionArray('p', "Properties",
            HelpText =
                "Set values to properties that will override properties in the file. Syntax steponthat.exe -p key1=value1 key2=value2"
            )]
        public string[] PropertyStrings { get; set; }

        public IEnumerable<Property> Properties { get; private set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public bool IsValid { get; private set; }

        public static Options TryParse(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                if (System.IO.File.Exists(options.File))
                {
                    options.Properties = options.PropertyStrings.Select(PropertyParser.Get);
                    options.IsValid = true;
                }
            }
            return options;
        }

    }
}