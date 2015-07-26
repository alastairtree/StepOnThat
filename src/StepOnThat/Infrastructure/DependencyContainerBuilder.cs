using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using StepOnThat.Steps;
using StepOnThat.Steps.Browser;
using StepOnThat.Steps.Browser.Actions;
using StepOnThat.Steps.Http;

namespace StepOnThat.Infrastructure
{
    public class DependencyContainerBuilder
    {
        internal ContainerBuilder Builder { get; private set; }

        public DependencyContainerBuilder(bool init = true)
        {
            if (init)
            {
                Init();
                Build();
            }
        }

        public IContainer Container { get; private set; }

        public void Init()
        {
            Builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            Builder.Register(x => new Instructions(x.Resolve<IHasProperties>()));

            //TODO: Make this be based on an attribute? [UsesVariables] perhaps?
            Builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (Step).IsAssignableFrom(t))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (PropertyInterceptor));

            Builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (BrowserAction).IsAssignableFrom(t))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (PropertyInterceptor));

            Builder.RegisterType<PropertyCollection>()
                .AsImplementedInterfaces()
                .SingleInstance();

            Builder.RegisterType<PropertyInterceptor>().AsSelf();

            Builder.RegisterType<StepRunner>().AsImplementedInterfaces();

            Builder.RegisterType<InstructionsRunner>().AsImplementedInterfaces();

            Builder.RegisterType<WebBrowser>().AsImplementedInterfaces().SingleInstance();

            Builder.RegisterType<HttpClient>().AsImplementedInterfaces();

            Builder.RegisterType<InstructionTypeFactory>().AsImplementedInterfaces();

            Builder.RegisterType<InstructionsReaderWriter>().AsSelf();

            Builder.RegisterType<Output>().AsImplementedInterfaces();

        }

        public void Build()
        {
            Container = Builder.Build();
        }
    }
}