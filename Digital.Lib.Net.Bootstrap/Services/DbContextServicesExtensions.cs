using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Digital.Lib.Net.Bootstrap.Services;

public static class DbContextServicesExtensions
{

    public static void EnsureDbContextCreated(this IServiceCollection services, ServiceDescriptor? descriptor = null)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        using var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetService(descriptor.ServiceType.GenericTypeArguments[0]) as DbContext;
        dbContext?.Database.EnsureCreated();
    }

    public static List<ServiceDescriptor> GetEfDescriptors(this IServiceCollection services) =>
        services
            .Where(s => s.ServiceType.FullName != null && s.ServiceType.FullName.StartsWith("Microsoft.EntityFrameworkCore"))
            .ToList();

    public static List<ServiceDescriptor> GetContextDescriptors(this List<ServiceDescriptor> services) =>
        services
            .Where(d => d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
            .ToList();

    public static MethodInfo GetAddDbContextMethod(this ServiceDescriptor descriptor) =>
        typeof(EntityFrameworkServiceCollectionExtensions)
            .GetMethods()
            .First(
                m =>
                    m is { Name: "AddDbContext", IsGenericMethodDefinition: true }
                    && m.GetGenericArguments().Length == 2
                    && m.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                    && m.GetParameters()[1].ParameterType == typeof(Action<DbContextOptionsBuilder>)
                    && m.GetParameters()[2].ParameterType == typeof(ServiceLifetime)
                    && m.GetParameters()[3].ParameterType == typeof(ServiceLifetime)
            )
            .MakeGenericMethod(
                descriptor.ServiceType.GenericTypeArguments[0],
                descriptor.ServiceType.GenericTypeArguments[0]
            );
}