using eCommerce.OperationalService.Domain.Operation.ValueObjects;
using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие изменения планового периода операции.
/// </summary>
public sealed record OperationPlannedPeriodChanged : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Entities.Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Новый плановый период операции.
    /// </summary>
    public required Period NewPlannedPeriod { get; init; }

    /// <summary>
    /// Создать событие изменения планового периода операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationPlannedPeriodChanged(
        Guid operationId,
        TimeProvider timeProvider
    )
    : base(
        operationId,
        timeProvider
    )
    {
    }
}