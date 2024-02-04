namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс для событий предметой области.
/// </summary>
/// <typeparam name="TAggregateId">
/// Тип идентификатора агрегата.
/// </typeparam>
public abstract record DomainEvent<TAggregateId> : Event
{
    /// <summary>
    /// Идентификатор агрегата,
    /// который опубликовал событие предметной области.
    /// </summary>
    public required TAggregateId AggregateRootId { get; init; }


    /// <inheritdoc/>
    public DomainEvent() : base() { }

    /// <inheritdoc/>
    public DomainEvent(TimeProvider timeProvider)
        : base(timeProvider) { }
}