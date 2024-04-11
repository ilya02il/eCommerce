namespace eCommerce.Common.Domain.Errors;

/// <summary>
/// Ошибки, возникающие в классе <see cref="AggregateRoot{TId}"/>.
/// </summary>
public static class AggregateRootErrors
{
    /// <summary>
    /// Невозможно опубликовать событие предметной области.
    /// Корень агрегата и событие предметной области относятся к разным агрегатам.
    /// </summary>
    public static readonly Error DomainEventHasDifferentAggregate = new(
        code: "DOMAIN-AGG-ROOT-001",
        message:
            "Невозможно опубликовать событие предметной области. " +
            "Корень агрегата и событие предметной области относятся к разным агрегатам."
    );

    /// <summary>
    /// Невозможно опубликовать событие предметной области.
    /// Версии агрегата у корня агрегата и события несовместимы.
    /// </summary>
    public static readonly Error DomainEventHasNotCompatibleAggregateVersion = new(
        code: "DOMAIN-AGG-ROOT-002",
        message:
            "Невозможно опубликовать событие предметной области. " +
            "Версии агрегата у корня агрегата и события несовместимы."
    );

    /// <summary>
    /// Невозможно опубликовать событие предметной области.
    /// Событие предметной области относится к другому экземпляру агрегата.
    /// </summary>
    public static readonly Error DomainEventFromAnotherAggregateInstance = new(
        code: "DOMAIN-AGG-ROOT-003",
        message:
            "Невозможно опубликовать событие предметной области. " +
            "Событие предметной области относится к другому экземпляру агрегата."
    );
}