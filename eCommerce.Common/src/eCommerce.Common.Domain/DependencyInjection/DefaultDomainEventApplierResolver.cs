using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Common.Domain.DependencyInjection;

/// <summary>
/// Реализация по-умолчанию резолвера
/// применителя события предметной области.
/// </summary>
/// <param name="serviceProvider">Провайдер сервисов.</param>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>л
internal class DefaultDomainEventApplierResolver<TRoot, TRootId>(
    IServiceProvider serviceProvider
) : IDomainEventApplierResolver<TRoot, TRootId>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <inheritdoc/>
    public IDomainEventApplier<TRoot, TRootId, TDomainEvent>? ResolveFor<TDomainEvent>()
        where TDomainEvent : DomainEvent<TRootId>
    {
        var domainEventApplier = serviceProvider
            .GetService<IDomainEventApplier<TRoot, TRootId, TDomainEvent>>();

        return domainEventApplier;
    }
}