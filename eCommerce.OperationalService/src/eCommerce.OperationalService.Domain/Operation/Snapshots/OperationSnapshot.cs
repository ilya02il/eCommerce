using eCommerce.OperationalService.Domain.Operation.Enums;
using eCommerce.OperationalService.Domain.Operation.ValueObjects;

namespace eCommerce.OperationalService.Domain.Operation.Snapshots;

/// <summary>
/// Снэпшот состояния операции.
/// </summary>
public record OperationSnapshot
{
    /// <summary>
    /// Идентификатор операции.
    /// </summary>
    public required Guid Identifier { get; init; }

    /// <summary>
    /// Название операции.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Описание операции.
    /// </summary>
    public required string? Description { get; init; }

    /// <summary>
    /// Статус операции.
    /// </summary>
    public required OperationStatus Status { get; init; }

    /// <summary>
    /// Плановый период выполнения операции.
    /// </summary>
    public required Period PlannedPeriod { get; init; }

    /// <summary>
    /// Фактический период выполнения операции.
    /// </summary>
    public required Period? ActualPeriod { get; init; }
}