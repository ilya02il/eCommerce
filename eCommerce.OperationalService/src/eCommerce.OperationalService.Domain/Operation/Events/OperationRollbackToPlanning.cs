﻿using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Events;

/// <summary>
/// Событие отката операции в планирование.
/// </summary>
public sealed record OperationRollbackToPlanning : DomainEvent<Guid>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Создать событие отката операции в планирование.
    /// </summary>
    /// <param name="operationId">
    /// Идентификатор операции,
    /// которую откатили в планирование.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public OperationRollbackToPlanning(
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