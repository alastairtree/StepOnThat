using System.IO;
using Newtonsoft.Json;
using StepOnThat.Browser.Actions;

namespace StepOnThat
{
    public static class InstructionsReaderWriter
    {
        static readonly JsonConverter[] Converters = 
            {
                new JsonTypePropertyConverter<Step> (typeof (Step)),
                new JsonTypePropertyConverter<BrowserAction>(typePropertyName:"action") 
            };

        public static string Write(Instructions instructions)
        {
            var text = JsonConvert.SerializeObject(instructions, Formatting.Indented);
            return text;
        }

        public static void WriteFile(Instructions instructions, string path)
        {
            var text = Write(instructions);
            File.WriteAllText(path, text);
        }

        public static Instructions ReadFile(string path)
        {
            var text = File.ReadAllText(path);
            return Read(text);
        }

        public static Instructions Read(string JSON)
        {
            return JsonConvert.DeserializeObject<Instructions>(JSON, Converters);
        }
    }
}