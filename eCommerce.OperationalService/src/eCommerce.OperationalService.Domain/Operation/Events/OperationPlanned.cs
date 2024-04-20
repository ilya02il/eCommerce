using eCommerce.OperationalService.Domain.Operation.ValueObjects;

using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие планирования операции.
/// </summary>
public sealed record OperationPlanned : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Название запланированной операции.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Описание запланированной операции.
    /// </summary>
    public required string? Description { get; init; }

    /// <summary>
    /// Плановый период запланированной операции.
    /// </summary>
    public required Period PlannedPeriod { get; init; }

    /// <summary>
    /// Создать событие планирования операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор запланированной операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationPlanned(
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