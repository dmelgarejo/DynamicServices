namespace Mvc.Application
{
	using System;
	using Castle.Windsor;
	using DynamicServices;
	using DynamicServices.Mvc;
	using DynamicServices.Mvc.ActionDescriptors;
	using DynamicServices.Sakurity;
	using Models;

	public class DynamicServicesRegistry : ServicesRegistry
	{
		public DynamicServicesRegistry()
		{
			Service("Products").For<IDynamicRepository<Product>>().For<ProductQueries>();
			Service("Product").Entity<Product>();
			Service("Locations").For<LocationQueries>();
		}

		public static void RegisterConventions()
		{
			DynamicControllerRegistrar.AddCommonActionFor<Query>("alls");
			DynamicControllerRegistrar.AddCommonActionFor<ViewWithoutModel>("index");
			DynamicControllerRegistrar.AddCommonActionFor<DynamicInsert>("add");
		}

		public static void Bootstrap(IWindsorContainer container)
		{
			var offica = container.Resolve<ISakurityOffica>();
			var sakurity = new Sakurity();
			sakurity.ConfigyaOffia(offica);
		}
	}
}