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
        public DependencyContainerBuilder(bool init = true)
        {
            if (init)
                Init();
        }

        public IContainer Container { get; private set; }

        public void Init()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            builder.Register(x => new Instructions(x.Resolve<IHasProperties>()));

            //TODO: Make this be based on an attribute? [UsesVariables] perhaps?
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (Step).IsAssignableFrom(t))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (PropertyInterceptor));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof (BrowserAction).IsAssignableFrom(t))
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof (PropertyInterceptor));

            builder.RegisterType<PropertyCollection>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<PropertyInterceptor>().AsSelf();

            builder.RegisterType<StepRunner>().AsImplementedInterfaces();

            builder.RegisterType<InstructionsRunner>().AsImplementedInterfaces();

            builder.RegisterType<WebBrowser>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HttpClient>().AsImplementedInterfaces();

            builder.RegisterType<InstructionTypeFactory>().AsImplementedInterfaces();

            builder.RegisterType<InstructionsReaderWriter>().AsSelf();

            Container = builder.Build();
        }
    }
}