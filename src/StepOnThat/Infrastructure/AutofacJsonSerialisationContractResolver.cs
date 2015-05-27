using System;
using Autofac;
using Newtonsoft.Json.Serialization;

namespace StepOnThat.Infrastructure
{
    public class AutofacJsonSerialisationContractResolver : DefaultContractResolver
    {
        private readonly ILifetimeScope container;

        public AutofacJsonSerialisationContractResolver(ILifetimeScope container)
        {
            this.container = container;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);

            // use Autofac to create types that have been registered with it
            if (container.IsRegistered(objectType))
                contract.DefaultCreator = () => container.Resolve(objectType);

            return contract;
        }
    }
}