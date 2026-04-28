using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeUsuarios.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddHandlersFromAssembly(assembly);

        return services;
    }

    private static void AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                     && t.Name.EndsWith("Handler", StringComparison.Ordinal));

        foreach (var type in handlerTypes)
        {
            services.AddScoped(type);
        }
    }
}
