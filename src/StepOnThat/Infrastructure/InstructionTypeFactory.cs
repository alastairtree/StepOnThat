using Autofac;
using System;

namespace StepOnThat.Infrastructure
{
    public class InstructionTypeFactory : IInstructionTypeFactory
    {
        private readonly ILifetimeScope container;

        public InstructionTypeFactory(ILifetimeScope container)
        {
            this.container = container;
        }

        public TType Build<TType>(Type typeToBuild)
        {
            return (TType) container.Resolve(typeToBuild);
        }

        public bool CanBuild(Type typeToCheck)
        {
            return container.IsRegistered(typeToCheck);
        }
    }
}