using Newtonsoft.Json.Serialization;
using System;

namespace StepOnThat.Infrastructure
{
    public class StepOnThatContractResolver : DefaultContractResolver
    {
        private readonly IInstructionTypeFactory typeBuilder;

        public StepOnThatContractResolver(IInstructionTypeFactory typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);

            // use Autofac to create types that have been registered with it
            if (typeBuilder.CanBuild(objectType))
                contract.DefaultCreator = () => typeBuilder.Build<object>(objectType);

            return contract;
        }
    }
}