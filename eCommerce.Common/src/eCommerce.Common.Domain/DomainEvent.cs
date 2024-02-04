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
    required public TAggregateId AggregateRootId { get; init; }

    /// <inheritdoc cref="Event()"/>
    protected DomainEvent()
        : base()
    {
    }

    /// <inheritdoc cref="Event(TimeProvider)"/>
    protected DomainEvent(TimeProvider timeProvider)
        : base(timeProvider)
    {
    }
}