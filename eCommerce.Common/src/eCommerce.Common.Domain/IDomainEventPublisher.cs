namespace eCommerce.Common.Domain;

/// <summary>
/// Интерфейс публикатора событий предметной области
/// для агрегата с типом корня <typeparamref name="TRoot"/>.
/// </summary>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
public interface IDomainEventPublisher<TRoot, TRootId>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Опубликовать событие предметной области
    /// <paramref name="domainEvent"/>
    /// для агрегата <paramref name="aggregate"/>.
    /// </summary>
    /// <param name="aggregate">
    /// Агрегат, для которого необходимо опубликовать событие предметной области.
    /// </param>
    /// <param name="domainEvent">
    /// Событие предметной области, которое необходимо опубликовать.
    /// </param>
    /// <typeparam name="TDomainEvent">
    /// Тип события предметной области, которое необходимо опубликовать.
    /// </typeparam>
    public Task Publish<TDomainEvent>(
        Aggregate<TRoot, TRootId> aggregate,
        TDomainEvent domainEvent
    )
    where TDomainEvent : DomainEvent<TRootId>;
}