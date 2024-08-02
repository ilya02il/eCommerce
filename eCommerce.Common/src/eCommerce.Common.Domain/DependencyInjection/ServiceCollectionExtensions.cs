using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace eCommerce.Common.Domain.DependencyInjection;

/// <summary>
/// Класс с методами расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить в коллекцию сервисов публикатор событий предметной области.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>
    /// Коллекцию сервисов с добавленным в нее публикатором
    /// событий предметной области.
    /// </returns>
    public static IServiceCollection AddDomainEventPublisher(this IServiceCollection services)
    {
        services.TryAddSingleton(
            typeof(IDomainEventPublisher<,>),
            typeof(DefaultDomainEventPublisher<,>)
        );

        services.TryAddSingleton(
            typeof(IDomainEventHandlersResolver<>),
            typeof(DefaultDomainEventHandlersResolver<>)
        );

        services.TryAddSingleton(
            typeof(IDomainEventApplierResolver<,>),
            typeof(DefaultDomainEventApplierResolver<,>)
        );

        return services;
    }

    /// <summary>
    /// Добавить в коллекцию сервисов все обработчики
    /// событий предметной области из указанной сборки
    /// (<paramref name="targetAssembly"/>).
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="targetAssembly">
    ///     Сборка, из которой необходимо добавить
    ///     обработчики событий предметной области.
    /// </param>
    /// <returns>
    /// Коллекцию сервисов с добавленными в нее из указанной
    /// сборки обработчиками событий предметной области.
    /// </returns>
    public static IServiceCollection AddDomainEventHandlers(
        this IServiceCollection services,
        Assembly targetAssembly
    )
    {
        services.AddImplementationsFromAssembly(
            typeof(IDomainEventHandler<,>),
            ServiceLifetime.Scoped,
            targetAssembly
        );

        return services;
    }

    /// <summary>
    /// Добавить в коллекцию сервисов все применители
    /// событий предметной области из указанной сборки
    /// (<paramref name="targetAssembly"/>)
    /// или текущей выполняющейся сборки,
    /// если <paramref name="targetAssembly"/> не указана.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="targetAssembly">
    ///     Сборка, из которой необходимо добавить
    ///     применители событий предметной области.
    /// </param>
    /// <returns>
    /// Коллекцию сервисов с добавленными в нее из указанной
    /// сборки применителями событий предметной области.
    /// </returns>
    public static IServiceCollection AddDomainEventAppliers(
        this IServiceCollection services,
        Assembly targetAssembly
    )
    {
        services.AddImplementationsFromAssembly(
            typeof(IDomainEventApplier<,,>),
            ServiceLifetime.Scoped,
            targetAssembly
        );

        return services;
    }

    private static void AddImplementationsFromAssembly(
        this IServiceCollection services,
        Type baseType,
        ServiceLifetime implementationsLifetime,
        Assembly targetAssembly
    )
    {
        var implementationsTypes = targetAssembly
            .DefinedTypes
            .Where((type) => type is
            {
                IsClass: true,
                IsAbstract: false
            })
            .Where((type) => Array
                .Exists(
                    type.GetInterfaces(),
                    (interfaceType) => interfaceType.MatchGenericTypeDefinition(baseType)
                )
            );

        foreach (var implementationType in implementationsTypes)
        {
            var baseTypes = implementationType
                .GetInterfaces()
                .Where((interfaceType) =>
                    interfaceType.MatchGenericTypeDefinition(baseType)
                );

            foreach (var interfaceType in baseTypes)
            {
                services.Add(new ServiceDescriptor(
                    interfaceType,
                    implementationType,
                    implementationsLifetime
                ));
            }
        }
    }

    private static bool MatchGenericTypeDefinition(
        this Type genericType,
        Type genericTypeDefinition
    )
    {
        return
            genericType.IsGenericType &&
            genericType.GetGenericTypeDefinition() == genericTypeDefinition;
    }
}