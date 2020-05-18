using Application.Common.Interfaces;
using Autofac;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace WebUI
{
    public class InfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //builder.RegisterAggregateService<IServiceBaseDependencyAggregate>();

            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("Infrastructure"))
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            //builder.Register(c => new UserProvider(c.Resolve<IHttpContextAccessor>())).As<IUserProvider>().InstancePerLifetimeScope();
        }
    }
}
