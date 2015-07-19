using System;

namespace StepOnThat.Infrastructure
{
    public interface IInstructionTypeFactory
    {
        TType Build<TType>(Type typeToBuild);
        bool CanBuild(Type typeToCheck);
    }
}