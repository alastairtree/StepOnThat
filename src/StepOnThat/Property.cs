namespace StepOnThat
{
    public struct Property
    {
        public Property(string key, string value) : this()
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}