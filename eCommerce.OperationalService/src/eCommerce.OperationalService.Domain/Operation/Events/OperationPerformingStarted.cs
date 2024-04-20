using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие начала выполнения операции.
/// </summary>
public sealed record OperationPerformingStarted : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Дата и время начала выполнения операции.
    /// </summary>
    public required DateTimeOffset PerformingStartDateTime { get; init; }

    /// <summary>
    /// Создать событие начала выполнения операции.
    /// </summary>
    /// <param name="operationId"></param>
    /// <param name="timeProvider"></param>
    public OperationPerformingStarted(
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