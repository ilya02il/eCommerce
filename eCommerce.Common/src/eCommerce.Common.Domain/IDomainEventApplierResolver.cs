namespace eCommerce.Common.Domain;

/// <summary>
/// Резолвер применителей событий предметной области
/// к агрегату с типом корня <typeparamref name="TRoot"/>.
/// </summary>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
public interface IDomainEventApplierResolver<in TRoot, TRootId>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Получить применитель для события предметной области
    /// с типом <typeparamref name="TDomainEvent"/>.
    /// </summary>
    /// <typeparam name="TDomainEvent">Тип события предметной области.</typeparam>
    /// <returns>
    /// Применитель для события предметной области с типом
    /// <typeparamref name="TDomainEvent"/> или <see langword="null"/>,
    /// если применитель не был найден.
    /// </returns>
    public IDomainEventApplier<TRoot, TRootId, TDomainEvent>? ResolveFor<TDomainEvent>()
        where TDomainEvent : DomainEvent<TRootId>;
}