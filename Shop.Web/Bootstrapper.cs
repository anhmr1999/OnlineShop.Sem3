using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Shop.EntityFramework;
using Shop.EntityFramework.Infrastructures;
using Shop.EntityFramework.Infrastructures.Repository;
using Shop.Web.Authentications;
using Unity.Mvc4;

namespace Shop.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            //var context = new ShopDbContext();
            //container.RegisterInstance<ShopDbContext>("ShopDbContext", context);
            container.RegisterType(typeof(DbContext), typeof(ShopDbContext));
            container.RegisterType(typeof(ICurrentPrincipal), typeof(CurrentPrincipal));
            container.RegisterType(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>), new TransientLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>), new TransientLifetimeManager());
        }
    }
}