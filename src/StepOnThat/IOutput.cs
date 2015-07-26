namespace StepOnThat
{
    public interface IOutput
    {
        void Write(string msg, params object[] args);
    }
}