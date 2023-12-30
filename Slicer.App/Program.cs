using System;
using Microsoft.Extensions.DependencyInjection;

namespace Slicer.App;

public class Program
{
    private static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<SlicerGame>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        using SlicerGame game = serviceProvider.GetRequiredService<SlicerGame>();

        game.Run();
    }
}