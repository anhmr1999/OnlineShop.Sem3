using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Shop.EntityFramework;
using Shop.EntityFramework.Infrastructures.Repository;
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
            container.RegisterInstance<ShopDbContext>(new ShopDbContext(), new TransientLifetimeManager());
            container.RegisterType(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>), new TransientLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>), new TransientLifetimeManager());
        }
  }
}