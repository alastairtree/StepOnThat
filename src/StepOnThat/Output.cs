using System;

namespace StepOnThat
{
    internal class Output : IOutput
    {
        public void Write(string msg, params object[] args)
        {
            if (args == null || args.Length == 0)
                Console.WriteLine(msg);
            else
            {
                Console.WriteLine(msg, args);
            }
        }
    }
}