using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие отката операции в выполнение.
/// </summary>
public sealed record OperationRollbackToPerforming : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Создать событие отката операции в выполнение.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции,
    /// которую откатили в выполнение.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationRollbackToPerforming(
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