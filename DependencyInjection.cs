using EstrellaAccesoriosWpf.Common;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EstrellaAccesoriosWpf;

public static class DependencyInjection
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        services.AddTransient<MainWindow>();
        services.AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>((x) => new SnackbarMessageQueue(TimeSpan.FromSeconds(3)));
        return services;
    }
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        string? cnnString = configuration.GetConnectionString("Production");
#if DEBUG
        cnnString = configuration.GetConnectionString("Development");
#endif
        services.AddDbContext<EstrellaDbContext>(options => options.UseSqlServer(cnnString));
        return services;
    }
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var viewTypes = assembly.GetTypes()
                                .Where(t => typeof(View).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var viewType in viewTypes)
        {
            services.AddTransient(typeof(View), viewType);
        }
        return services;
    }
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var viewTypes = assembly.GetTypes()
                                .Where(t => typeof(ViewModel).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var viewType in viewTypes)
        {
            services.AddScoped(viewType);
        }
        return services;
    }
}
