using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Slicer.App.Builders;
using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Slicer.App;

public class Program
{
    private static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<SlicerGame>();

        services.AddSlicerGameServices();
        services.AddEntities();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        using SlicerGame game = serviceProvider.GetRequiredService<SlicerGame>();

        game.Run();
    }
}