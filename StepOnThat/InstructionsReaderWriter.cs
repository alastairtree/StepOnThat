using System.IO;
using Newtonsoft.Json;

namespace StepOnThat
{
    public static class InstructionsReaderWriter
    {
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
            return JsonConvert.DeserializeObject<Instructions>(JSON);
        }
    }
}