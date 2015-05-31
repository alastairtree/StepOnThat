using System;

namespace StepOnThat
{
    public static class PropertyParser
    {
        public static Property Get(string pairString)
        {
            var splits = pairString.Trim().Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (splits.Length > 2 || splits.Length < 1)
                throw new ApplicationException(
                    String.Format("Unable to parse '{0}' into a key and value pair. Ensure syntax is 'key=value'",
                        pairString));

            var key = splits[0].Trim();

            string value = splits.Length == 2 ? splits[1] : "";

            return new Property(key, value);
        }
    }
}