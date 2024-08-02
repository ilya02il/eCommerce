namespace eCommerce.Common.Domain;

/// <summary>
/// Обработчик события предметной области
/// с типом <typeparamref name="TDomainEvent"/>.
/// </summary>
/// <typeparam name="TDomainEvent">Тип события предметной области.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
public interface IDomainEventHandler<in TDomainEvent, TRootId>
    where TDomainEvent : DomainEvent<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Обработать событие предметной области.
    /// </summary>
    /// <param name="domainEvent">Событие предметной области.</param>
    public Task Handle(TDomainEvent domainEvent);
}