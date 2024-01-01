using System.Reflection;
using Slicer.App.Builders;
using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddSlicerGameServices(this IServiceCollection services)
	{
		services.AddSingleton<IEntityManagerService, EntityManagerService>();

		services.AddTransient<IAnimationHandlerServiceBuilder, AnimationHandlerServiceBuilder>();
		services.AddTransient<IPhysicsHandlerServiceBuilder, PhysicsHandlerServiceBuilder>();
		services.AddTransient<IAttackHandlerServiceBuilder, AttackHandlerServiceBuilder>();
		services.AddTransient<IHealthHandlerServiceBuilder, HealthHandlerServiceBuilder>();

		return services;
	}

	public static IServiceCollection AddEntities(this IServiceCollection services)
	{
		Assembly assembly = Assembly.GetExecutingAssembly();

		List<Type> entityTypes = assembly
			.GetTypes()
			.Where(x => x.IsAssignableTo(typeof(IEntity)) && x.IsClass)
			.ToList();

		foreach (var entityType in entityTypes)
		{
			services.AddTransient(entityType);
		}

		return services;
	}
}