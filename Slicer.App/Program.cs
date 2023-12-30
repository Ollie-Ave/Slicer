using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Slicer.App;

public class Program
{
    private static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<SlicerGame>();
        services.AddSingleton<IEntityManagerService, EntityManagerService>();

        services.AddTransient<IAnimationHandlerService, AnimationHandlerService>();

        services = RegisterEntities(services);

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        using SlicerGame game = serviceProvider.GetRequiredService<SlicerGame>();

        game.Run();
    }

    public static IServiceCollection RegisterEntities(IServiceCollection services)
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