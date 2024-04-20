using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие завершения операции.
/// </summary>
public sealed record OperationCompleted : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Дата и время завершения операции.
    /// </summary>
    public required DateTimeOffset CompletionDateTime { get; init; }

    /// <summary>
    /// Создать событие завершения операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationCompleted(
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