namespace eCommerce.Common.Domain.Errors;

/// <summary>
/// Ошибки, возникающие в <see cref="DefaultDomainEventPublisher{TRoot,TRootId}"/>.
/// </summary>
internal static class DefaultDomainEventPublisherErrors
{
    /// <summary>
    /// <c>COMMON-DOMAIN-EVENT-PUB-001</c>:
    /// <br/>
    /// Невозможно опубликовать событие предметной области.
    /// Событие предметной области относится к другому экземпляру агрегата.
    /// </summary>
    public static readonly Error DomainEventFromAnotherAggregateInstance = new(
        code: "COMMON-DOMAIN-EVENT-PUB-001",
        message:
            "Невозможно опубликовать событие предметной области. " +
            "Событие предметной области относится к другому экземпляру агрегата."
    );
}