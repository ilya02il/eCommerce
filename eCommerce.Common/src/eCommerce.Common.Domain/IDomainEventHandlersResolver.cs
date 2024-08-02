namespace eCommerce.Common.Domain;

/// <summary>
/// Резолвер обработчиков для событий предметной области.
/// </summary>
/// <typeparam name="TRootId">Тип корня агрегата.</typeparam>
public interface IDomainEventHandlersResolver<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Получить коллекцию обработчиков для события
    /// предметной области с типом <typeparamref name="TDomainEvent"/>.
    /// </summary>
    /// <typeparam name="TDomainEvent">Тип события предметной области.</typeparam>
    /// <returns>
    /// Коллекцию обработчиков для события
    /// предметной области с типом <typeparamref name="TDomainEvent"/>.
    /// </returns>
    public IEnumerable<IDomainEventHandler<TDomainEvent, TRootId>> ResolveFor<TDomainEvent>()
        where TDomainEvent : DomainEvent<TRootId>;
}