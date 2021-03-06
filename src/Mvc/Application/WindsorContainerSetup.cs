using CommonServiceLocator.WindsorAdapter;

namespace Mvc.Application
{
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using Microsoft.Practices.ServiceLocation;

	public class WindsorContainerSetup
	{
		private static object _lock = new object();

		public static IWindsorContainer Container;

		public static bool InitializeContainer()
		{
			lock (_lock)
			{
				if (Container != null)
				{
					return false;
				}
				Container = new WindsorContainer();
				Container.Kernel.Resolver.AddSubResolver(new ListResolver(Container.Kernel));
				Container.Register(
					Component.For<IWindsorContainer>()
						.Instance(Container)
						.LifeStyle.Singleton
					);
				ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(Container));
				Container.Register(Component.For<IServiceLocator>().Instance(ServiceLocator.Current).LifeStyle.Singleton);
			}
			return true;
		}
	}
}