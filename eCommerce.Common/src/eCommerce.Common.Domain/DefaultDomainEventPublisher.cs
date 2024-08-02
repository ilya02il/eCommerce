using eCommerce.Common.Domain.Errors;

namespace eCommerce.Common.Domain;

/// <summary>
/// Реализация по-умолчанию публикатора событий предметной области.
/// </summary>
/// <param name="domainEventHandlersResolver">
/// Резолвер обработчиков событий предметной области.
/// </param>
/// <param name="domainEventApplierResolver">
/// Резолвер применителей событий предметной области.
/// </param>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
internal sealed class DefaultDomainEventPublisher<TRoot, TRootId>(
    IDomainEventHandlersResolver<TRootId> domainEventHandlersResolver,
    IDomainEventApplierResolver<TRoot, TRootId> domainEventApplierResolver
) : IDomainEventPublisher<TRoot, TRootId>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <inheritdoc/>
    /// <exception cref="DefaultDomainEventPublisherErrors.DomainEventFromAnotherAggregateInstance">
    /// Ошибка, которая будет выброшена, когда идентификатор
    /// корня агрегата не совпадает с таковым в событии предметной области.
    /// </exception>
    public async Task Publish<TDomainEvent>(
        Aggregate<TRoot, TRootId> aggregate,
        TDomainEvent domainEvent
    )
    where TDomainEvent : DomainEvent<TRootId>
    {
        if (aggregate.Root.Id.Equals(domainEvent.AggregateRootId) is false)
        {
            throw DefaultDomainEventPublisherErrors
                .DomainEventFromAnotherAggregateInstance;
        }

        var handleTasks = domainEventHandlersResolver
            .ResolveFor<TDomainEvent>()
            .Select((handler) => handler.Handle(domainEvent));

        await Task.WhenAll(handleTasks).ConfigureAwait(false);

        var applier = domainEventApplierResolver.ResolveFor<TDomainEvent>();
        applier?.Apply(aggregate.Root, domainEvent);

        aggregate.UncommittedEvents.Enqueue(domainEvent);
    }
}