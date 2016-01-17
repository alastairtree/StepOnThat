using Newtonsoft.Json;
using StepOnThat.Infrastructure;
using StepOnThat.Steps;
using StepOnThat.Steps.Browser.Actions;
using System.IO;
using System.Text.RegularExpressions;

namespace StepOnThat
{
    public class InstructionsReaderWriter
    {
        private JsonSerializerSettings settings;

        public InstructionsReaderWriter(IInstructionTypeFactory typeBuilder)
        {
            JsonConverter[] converters =
            {
                new JsonTypePropertyConverter<Step>(typeBuilder, defaultyValueType: typeof (Step),
                    ignorePatternInTypeName: @"(?<=\w)[sS]tep$"),
                new JsonTypePropertyConverter<BrowserAction>(typeBuilder, typePropertyDiscriminatorName: "action")
            };
            var contractResolver = new StepOnThatContractResolver(typeBuilder);
            settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Converters = converters
            };
        }

        public string Write(Instructions instructions)
        {
            var text = JsonConvert.SerializeObject(instructions, Formatting.Indented);
            return text;
        }

        public void WriteFile(Instructions instructions, string path)
        {
            var text = Write(instructions);
            File.WriteAllText(path, text);
        }

        public Instructions ReadFile(string path)
        {
            var text = File.ReadAllText(path);
            return Read(text);
        }

        public Instructions Read(string JSON)
        {
            if (Regex.IsMatch(JSON, @"^\s*\[.*\]\s*$", RegexOptions.Singleline))
                JSON = string.Format("{{'steps':{0}}}", JSON);

            return JsonConvert.DeserializeObject<Instructions>(JSON, settings);
        }
    }
}