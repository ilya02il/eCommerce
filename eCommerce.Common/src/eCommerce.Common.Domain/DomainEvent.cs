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
    public TAggregateId AggregateRootId { get; private init; }

    /// <summary>
    /// Создать новый экземпляр события предметной области
    /// с указанием идентификатора агрегата,
    /// опубликовавшего это событие.
    /// </summary>
    /// <param name="aggregateRootId">
    /// Идентификатор агрегата, опубликовавшего событие.
    /// </param>
    protected DomainEvent(TAggregateId aggregateRootId)
    : base()
    {
        AggregateRootId = aggregateRootId;
    }

    /// <summary>
    /// Создать новый экземпляр события предметной области,
    /// в качестве провайдера для инциализации <see cref="Event.DateStamp"/>
    /// которого будет использоваться <paramref name="timeProvider"/>,
    /// также указать идентификатор агрегата, опубликовавшего это событие.
    /// </summary>
    /// <param name="aggregateRootId">
    /// Идентификатор агрегата, опубликовавшего событие.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени для инициализации <see cref="Event.DateStamp"/>.
    /// </param>
    protected DomainEvent(
        TAggregateId aggregateRootId,
        TimeProvider timeProvider
    )
    : base(timeProvider)
    {
        AggregateRootId = aggregateRootId;
    }
}
