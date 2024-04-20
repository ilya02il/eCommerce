using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие изменения названия операции.
/// </summary>
public sealed record OperationNameChanged : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Entities.Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Новое название операции.
    /// </summary>
    public required string NewName { get; init; }

    /// <summary>
    /// Создать событие изменения названия операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationNameChanged(
        Guid operationId,
        TimeProvider timeProvider
    )
    : base(operationId, timeProvider)
    {
    }
}