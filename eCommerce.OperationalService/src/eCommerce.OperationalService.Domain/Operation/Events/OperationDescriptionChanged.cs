using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие изменения описания операции.
/// </summary>
public sealed record OperationDescriptionChanged : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Entities.Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Новое описание операции.
    /// </summary>
    public required string? NewDescription { get; init; }

    /// <summary>
    /// Создать событие изменения описания операции.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationDescriptionChanged(
        Guid operationId,
        TimeProvider timeProvider
    )
    : base(operationId, timeProvider)
    {
    }
}