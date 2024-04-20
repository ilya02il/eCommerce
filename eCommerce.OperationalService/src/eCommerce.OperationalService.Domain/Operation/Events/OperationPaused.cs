using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие приостановки операции.
/// </summary>
public sealed record OperationPaused : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Создать событие приостановки операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationPaused(
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