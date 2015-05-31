using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using StepOnThat.Browser;
using StepOnThat.Browser.Actions;
using StepOnThat.Http;

namespace StepOnThat.Infrastructure
{
    public class DependencyResolver
    {
        private ContainerBuilder builder;

        public DependencyResolver(bool init = true)
        {
            if (init)
                Init();
        }

        public IContainer Container { get; private set; }

        public void Init()
        {
            builder = new ContainerBuilder();

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

            Container = builder.Build();
        }
    }
}