namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс для агрегатов.
/// </summary>
/// <typeparam name="TId">
/// Тип идентификатора агрегата.
/// </typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly Queue<DomainEvent<TId>> _domainEventsQueue = [];

    /// <summary>
    /// Создать новый экземпляр агрегата с
    /// идентификатором <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">
    /// Идентификатор агрегата.
    /// </param>
    protected AggregateRoot(TId identifier)
        : base(identifier)
    {
    }

    /// <summary>
    /// Коллекция для чтения событий доменной области,
    /// опубликованных агрегатом.
    /// </summary>
    public IReadOnlyCollection<DomainEvent<TId>> DomainEvents =>
        _domainEventsQueue;

    /// <summary>
    /// Опубликовать и применить к агрегату
    /// событие предметной области.
    /// </summary>
    /// <param name="domainEvent">
    /// Событие предметой области,
    /// которое необходимо опубликовать и
    /// применить к агрегату.
    /// </param>
    public void PublishDomainEvent(DomainEvent<TId> domainEvent)
    {
        _domainEventsQueue.Enqueue(domainEvent);
        ApplyDomainEvent(domainEvent);
    }

    /// <summary>
    /// Применить событие предметной области к агрегату.
    /// </summary>
    /// <param name="domainEvent">
    /// Событие предметной области,
    /// которое необходимо применить к агрегату.
    /// </param>
    public abstract void ApplyDomainEvent(DomainEvent<TId> domainEvent);
}