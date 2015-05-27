using System.Reflection;
using Autofac;
using StepOnThat.Browser.Actions;

namespace StepOnThat.Infrastructure
{
    public class DependencyResolver
    {
        private ContainerBuilder builder;

        public DependencyResolver(bool init = true)
        {
            if(init)
                Init();
        }

        public void Init()
        {
            builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterType<Instructions>().AsSelf();

            //TODO: Make this be based on an attribute? [UsesVariables] perhaps?
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (Step).IsAssignableFrom(t))
                .AsSelf();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (BrowserAction).IsAssignableFrom(t))
                .AsSelf();

            Container = builder.Build();

        }

        public IContainer Container { get; private set; }
    }
}