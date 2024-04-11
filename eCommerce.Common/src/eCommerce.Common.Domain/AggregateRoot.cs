using eCommerce.Common.Domain.Errors;

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
    /// Применить событие предметной области к агрегату.
    /// </summary>
    /// <param name="domainEvent">
    /// Событие предметной области,
    /// которое необходимо применить к агрегату.
    /// </param>
    /// <param name="dateTimeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public void ApplyDomainEvent(
        DomainEvent<TId> domainEvent,
        TimeProvider dateTimeProvider
    )
    {
        if (domainEvent.AggregateName != AggregateName)
        {
            throw AggregateRootErrors
                .DomainEventHasDifferentAggregate;
        }

        if (domainEvent.AggregateVersion.IsCompatibleWith(AggregateVersion) is false)
        {
            var requiredVersion = new Version(AggregateVersion.Major);

            throw AggregateRootErrors
                .DomainEventHasNotCompatibleAggregateVersion
                .AppendMessage($"Необходима версия агрегата >= {requiredVersion}.");
        }

        if (domainEvent.AggregateRootId!.Equals(Id) is false)
        {
            throw AggregateRootErrors
                .DomainEventFromAnotherAggregateInstance;
        }

        ApplyDomainEventInternal(domainEvent, dateTimeProvider);
    }

    /// <summary>
    /// Применить событие предметной области к агрегату.
    /// </summary>
    /// <param name="domainEvent">
    /// Событие предметной области,
    /// которое необходимо применить к агрегату.
    /// </param>
    /// <param name="dateTimeProvider">
    /// Провайдер даты и времени.
    /// </param>
    protected abstract void ApplyDomainEventInternal(
        DomainEvent<TId> domainEvent,
        TimeProvider dateTimeProvider
    );

    /// <summary>
    /// Опубликовать и применить к агрегату
    /// событие предметной области.
    /// </summary>
    /// <param name="domainEvent">
    /// Событие предметой области,
    /// которое необходимо опубликовать и
    /// применить к агрегату.
    /// </param>
    /// <param name="dateTimeProvider">
    /// Провайдер даты и времени.
    /// </param>
    protected void PublishDomainEvent(
        DomainEvent<TId> domainEvent,
        TimeProvider dateTimeProvider
    )
    {
        ApplyDomainEvent(domainEvent, dateTimeProvider);
        _domainEventsQueue.Enqueue(domainEvent);
    }
}