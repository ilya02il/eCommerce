using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Common.Domain.DependencyInjection;

/// <summary>
/// Реализация по-умолчанию резолвера
/// обработчиков события предметной области.
/// </summary>
/// <param name="serviceProvider">Провайдер сервисов.</param>
/// <typeparam name="TRootId">Тип корня агрегата.</typeparam>
internal class DefaultDomainEventHandlersResolver<TRootId>(
    IServiceProvider serviceProvider
) : IDomainEventHandlersResolver<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <inheritdoc/>
    public IEnumerable<IDomainEventHandler<TDomainEvent, TRootId>> ResolveFor<TDomainEvent>()
        where TDomainEvent : DomainEvent<TRootId>
    {
        var domainEventHandlers = serviceProvider
            .GetServices<IDomainEventHandler<TDomainEvent, TRootId>>();

        return domainEventHandlers;
    }
}