namespace Mvc.Application
{
	using DynamicServices;
	using DynamicServices.Sakurity;
	using Models;

	public class Sakurity : SakurityRegistry
	{
		public Sakurity()
		{
			DefaultLevel = 1;

			For<IDynamicRepository<Product>>()
				.Allow("All")
				.Everyone();

		}
	}
}