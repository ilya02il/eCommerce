namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс для событий предметой области.
/// </summary>
/// <typeparam name="TRootId">
/// Тип идентификатора корня агрегата.
/// </typeparam>
public abstract record DomainEvent<TRootId> : Event
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Идентификатор корня агрегата,
    /// опубликовавшего событие предметной области.
    /// </summary>
    public required TRootId AggregateRootId { get; init; }
}
