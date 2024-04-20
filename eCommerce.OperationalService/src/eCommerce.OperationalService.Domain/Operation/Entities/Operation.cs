using eCommerce.Common.Abstractions;

using eCommerce.OperationalService.Domain.Operation.Enums;
using eCommerce.OperationalService.Domain.Operation.Errors;
using eCommerce.OperationalService.Domain.Operation.Events;
using eCommerce.OperationalService.Domain.Operation.Snapshots;
using eCommerce.OperationalService.Domain.Operation.ValueObjects;

using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.Entities;

/// <summary>
/// Сущность операции.
/// </summary>
public class Operation :
    AggregateRoot<Guid>,
    IMemoizable<Operation, OperationSnapshot>
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Идентификатор операции.
    /// </summary>
    public Guid Identifier { get; }

    /// <summary>
    /// Название операции.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание операции.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Статус операции.
    /// </summary>
    public OperationStatus Status { get; private set; } =
        OperationStatus.Planning;

    /// <summary>
    /// Плановый период операции.
    /// </summary>
    public Period PlannedPeriod { get; private set; }

    /// <summary>
    /// Фактический период операции.
    /// </summary>
    public Period? ActualPeriod { get; private set; }

    private Operation(
        Guid identifier,
        string name,
        Period plannedPeriod
    ) : base(identifier)
    {
        Identifier = identifier;
        Name = name;
        PlannedPeriod = plannedPeriod;
    }
    
    private Operation(
        Guid identifier,
        string name,
        string? description,
        Period plannedPeriod,
        TimeProvider timeProvider
    ) : base(identifier)
    {
        Identifier = identifier;
        Name = name;
        Description = description;
        PlannedPeriod = plannedPeriod;

        PublishDomainEvent(
            new OperationPlanned(Identifier, timeProvider)
            {
                Name = name,
                Description = description,
                PlannedPeriod = plannedPeriod
            },
            timeProvider
        );
    }

    /// <summary>
    /// Запланировать оперцию.
    /// </summary>
    /// <param name="name">
    /// Название операции.
    /// </param>
    /// <param name="description">
    /// Описание операции.
    /// </param>
    /// <param name="plannedPeriod">
    /// Период планируемой оперции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <returns>
    /// Запланированную операцию.
    /// </returns>
    public static Operation Plan(
        string name,
        string? description,
        Period plannedPeriod,
        TimeProvider timeProvider
    )
    {
        var plannedOperation = new Operation(
            identifier: Guid.NewGuid(),
            name,
            description,
            plannedPeriod,
            timeProvider
        );

        return plannedOperation;
    }

    /// <summary>
    /// Начать выполнение операции.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    public void Perform(TimeProvider timeProvider)
    {
        var performingBeginningDateTime = timeProvider.GetUtcNow();

        PublishDomainEvent(
            new OperationPerformingStarted(Identifier, timeProvider)
            {
                PerformingStartDateTime = performingBeginningDateTime
            },
            timeProvider
        );
    }

    /// <summary>
    /// Приостановить выполнение операции.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.Status.CannotPauseIfTheStatusIsNotPerforming"/>
    public void Pause(TimeProvider timeProvider)
    {
        if (Status is not OperationStatus.Performing)
        {
            throw OperationErrors
                .Status
                .CannotPauseIfTheStatusIsNotPerforming;
        } 
        
        PublishDomainEvent(
            new OperationPaused(Identifier, timeProvider),
            timeProvider
        );
    }

    /// <summary>
    /// Продолжить выполнение операции.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.Status.CannotResumeIfTheStatusIsNotPaused"/>
    public void Resume(TimeProvider timeProvider)
    {
        if (Status is not OperationStatus.Paused)
        {
            throw OperationErrors
                .Status
                .CannotResumeIfTheStatusIsNotPaused;
        }
        
        PublishDomainEvent(
            new OperationResumed(Identifier, timeProvider),
            timeProvider
        );
    }

    /// <summary>
    /// Откатить операцию в предыдущий статус.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.Status.CannotRollbackIfTheStatusIsNotPerformingOrCompleted"/>
    public void Rollback(TimeProvider timeProvider)
    {
        if (Status is OperationStatus.Performing)
        {
            PublishDomainEvent(
                new OperationRollbackToPlanning(Identifier, timeProvider),
                timeProvider
            );

            return;
        }

        if (Status is OperationStatus.Completed)
        {
            PublishDomainEvent(
                new OperationRollbackToPerforming(Identifier, timeProvider),
                timeProvider
            );

            return;
        }

        throw OperationErrors
            .Status
            .CannotRollbackIfTheStatusIsNotPerformingOrCompleted;
    }

    /// <summary>
    /// Завершить операцию.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.Status.CannotCompleteIfTheStatusIsNotPerforming"/>
    public void Complete(TimeProvider timeProvider)
    {
        if (Status is not OperationStatus.Performing)
        {
            throw OperationErrors
                .Status
                .CannotCompleteIfTheStatusIsNotPerforming;
        }

        var completionDateTime = timeProvider.GetUtcNow();
        
        PublishDomainEvent(
            new OperationCompleted(Identifier, timeProvider)
            {
                CompletionDateTime = completionDateTime
            },
            timeProvider
        );
    }

    /// <summary>
    /// Изменить название операции.
    /// </summary>
    /// <param name="newName">
    /// Новое название операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.CannotChangeANameIfAStatusIsNotPlanning"/>
    public void ChangeName(
        string newName,
        TimeProvider timeProvider
    )
    {
        if (Status is not OperationStatus.Planning)
        {
            throw OperationErrors
                .CannotChangeANameIfAStatusIsNotPlanning;
        }
        
        PublishDomainEvent(
            new OperationNameChanged(Identifier, timeProvider)
            {
                NewName = newName
            },
            timeProvider
        );
    }

    /// <summary>
    /// Изменить описание операции.
    /// </summary>
    /// <param name="newDescription">
    /// Новое описание операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.CannotChangeADescriptionIfAStatusIsNotPlanning"/>
    public void ChangeDescription(
        string newDescription,
        TimeProvider timeProvider
    )
    {
        if (Status is not OperationStatus.Planning)
        {
            throw OperationErrors
                .CannotChangeADescriptionIfAStatusIsNotPlanning;
        }
        
        PublishDomainEvent(
            new OperationDescriptionChanged(Identifier, timeProvider)
            {
                NewDescription = newDescription
            },
            timeProvider);
    }

    /// <summary>
    /// Изменить плановый период операции.
    /// </summary>
    /// <param name="newPeriod">
    /// Новый плановый период операции.
    /// </param>
    /// <param name="timeProvider">
    /// Провайдер даты и времени.
    /// </param>
    /// <exception cref="OperationErrors.Period.CannotChangeAPlannedPeriodBeginningDateTime"/>
    public void ChangePlannedPeriod(
        Period newPeriod,
        TimeProvider timeProvider
    )
    {
        if (Status is not OperationStatus.Planning)
        {
            throw OperationErrors
                .Period
                .CannotChangeAPlannedPeriodBeginningDateTime
                .AppendMessage($"Текущий статус: {Status}.");
        }

        PublishDomainEvent(
            new OperationPlannedPeriodChanged(Identifier, timeProvider)
            {
                NewPlannedPeriod = newPeriod
            },
            timeProvider
        );
    }

    /// <inheritdoc />
    protected override void ApplyDomainEventInternal(
        DomainEvent<Guid> domainEvent,
        TimeProvider timeProvider
    )
    {
        switch (domainEvent)
        {
            case OperationPerformingStarted performingStartedEvent:
                ApplyPerformingStartedEvent(performingStartedEvent);

                break;
            case OperationPaused:
                ApplyPausedEvent();

                break;
            case OperationResumed:
                ApplyResumedEvent();

                break;
            case OperationCompleted completedEvent:
                ApplyCompletedEvent(completedEvent);

                break;
            case OperationRollbackToPlanning:
                ApplyRollbackToPlanningEvent();

                break;
            case OperationRollbackToPerforming:
                ApplyRollbackToPerformingEvent();

                break;
            case OperationPlannedPeriodChanged plannedPeriodChangedEvent:
                ApplyPlannedPeriodChangedEvent(plannedPeriodChangedEvent);

                break;
            case OperationNameChanged nameChangedEvent:
                ApplyNameChangedEvent(nameChangedEvent);

                break;
            case OperationDescriptionChanged descriptionChangedEvent:
                ApplyDescriptionChangedEvent(descriptionChangedEvent);

                break;
        }
    }

    private void ApplyPerformingStartedEvent(
        OperationPerformingStarted domainEvent
    )
    {
        Status = OperationStatus.Performing;
        
        ActualPeriod = new Period(
            start: domainEvent.PerformingStartDateTime,
            end: null
        );
    }

    private void ApplyPausedEvent()
    {
        Status = OperationStatus.Paused;
    }

    private void ApplyRollbackToPlanningEvent()
    {
        Status = OperationStatus.Planning;
        ActualPeriod = null;
    }

    private void ApplyRollbackToPerformingEvent()
    {
        Status = OperationStatus.Performing;
        ActualPeriod = new Period(ActualPeriod!.Start, end: null);
    }

    private void ApplyResumedEvent()
    {
        Status = OperationStatus.Performing;
    }

    private void ApplyCompletedEvent(OperationCompleted domainEvent)
    {
        Status = OperationStatus.Completed;
        
        ActualPeriod = new Period(
            ActualPeriod!.Start,
            domainEvent.CompletionDateTime
        );
    }

    private void ApplyPlannedPeriodChangedEvent(
        OperationPlannedPeriodChanged domainEvent
    )
    {
        PlannedPeriod = domainEvent.NewPlannedPeriod;
    }

    private void ApplyNameChangedEvent(
        OperationNameChanged domainEvent
    )
    {
        Name = domainEvent.NewName;
    }

    private void ApplyDescriptionChangedEvent(
        OperationDescriptionChanged domainEvent
    )
    {
        Description = domainEvent.NewDescription;
    }

    /// <inheritdoc/>
    public static Operation Restore(OperationSnapshot memento)
    {
        var restoredOperation = new Operation(
            memento.Identifier,
            memento.Name,
            memento.PlannedPeriod
        )
        {
            Description = memento.Description,
            Status = memento.Status,
            ActualPeriod = memento.ActualPeriod
        };

        return restoredOperation;
    }

    /// <inheritdoc/>
    public OperationSnapshot Memoize()
    {
        return new OperationSnapshot
        {
            Identifier = Identifier,
            Name = Name,
            Description = Description,
            Status = Status,
            PlannedPeriod = PlannedPeriod,
            ActualPeriod = ActualPeriod
        };
    }
}
