using System.IO;
using Newtonsoft.Json;

namespace StepOnThat
{
    public static class InstructionsReaderWriter
    {
        public static void WriteFile(Instructions instructions, string path)
        {
            var text = JsonConvert.SerializeObject(instructions, Formatting.Indented);
            File.WriteAllText(path, text);
        }

        public static Instructions ReadFile(string path)
        {
            var text = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Instructions>(text);
        }
    }
}