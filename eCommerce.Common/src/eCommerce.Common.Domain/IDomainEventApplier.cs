namespace eCommerce.Common.Domain;

/// <summary>
/// Интерфейс для класса, применяющего событие предметной области
/// с типом <typeparamref name="TDomainEvent"/>
/// к корню агрегата с типом <typeparamref name="TRoot"/>.
/// </summary>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
/// <typeparam name="TDomainEvent">Тип события предметной области.</typeparam>
public interface IDomainEventApplier<in TRoot, TRootId, in TDomainEvent>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
    where TDomainEvent : DomainEvent<TRootId>
{
    /// <summary>
    /// Применить событие предметной области к корню агрегата.
    /// </summary>
    /// <param name="aggregateRoot">Корень агрегата.</param>
    /// <param name="domainEvent">Событие предметной области.</param>
    public void Apply(TRoot aggregateRoot, TDomainEvent domainEvent);
}