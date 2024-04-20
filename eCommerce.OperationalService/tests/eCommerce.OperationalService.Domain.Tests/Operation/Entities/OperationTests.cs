using eCommerce.Common;
using eCommerce.OperationalService.Domain.Operation.Enums;
using eCommerce.OperationalService.Domain.Operation.Errors;
using eCommerce.OperationalService.Domain.Operation.Events;
using eCommerce.OperationalService.Domain.Operation.Snapshots;
using eCommerce.OperationalService.Domain.Operation.ValueObjects;

using OperationEntity =
    eCommerce.OperationalService.Domain.Operation.Entities.Operation;

namespace eCommerce.OperationalService.Tests.Operation.Entities;

/// <summary>
/// Класс с тестами сущности операции.
/// </summary>
public class OperationTests
{
    private const string OperationName = "test";
    private const string OperationDescription = "test";

    /// <summary>
    /// Сущность операции должна иметь правильные
    /// название агрегата и его версию.
    /// </summary>
    [Fact]
    public void Should_Have_Correct_Aggregate_Name_And_Version()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Assert
        operation
            .AggregateName
            .Should()
            .Be(nameof(Operation));

        operation
            .AggregateVersion
            .Major
            .Should()
            .Be(1);
    }

    /// <summary>
    /// Операция должна планироваться корректно.
    /// </summary>
    [Fact]
    public void Should_Plan_Correctly()
    {
        // Arrange
        var plannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddHours(1)
        );

        // Act
        var plannedOperation = OperationEntity.Plan(
            OperationName,
            OperationDescription,
            plannedPeriod,
            Mocks.TimeProvider
        );
        
        // Assert
        plannedOperation
            .Identifier
            .Should()
            .NotBeEmpty();

        plannedOperation
            .Name
            .Should()
            .Be(OperationName);

        plannedOperation
            .Description
            .Should()
            .Be(OperationDescription);

        plannedOperation
            .PlannedPeriod
            .Should()
            .Be(plannedPeriod);

        plannedOperation
            .Status
            .Should()
            .Be(OperationStatus.Planning);

        plannedOperation
            .ActualPeriod
            .Should()
            .Be(null);

        var operationPlannedEvent = (plannedOperation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationPlanned)
            .Which as OperationPlanned)!;

        operationPlannedEvent
            .AggregateRootId
            .Should()
            .Be(plannedOperation.Identifier);

        operationPlannedEvent
            .Name
            .Should()
            .Be(OperationName);

        operationPlannedEvent
            .Description
            .Should()
            .Be(OperationDescription);

        operationPlannedEvent
            .PlannedPeriod
            .Should()
            .Be(plannedPeriod);
    }

    /// <summary>
    /// Начало выполнения операции должно быть корректным.
    /// </summary>
    [Fact]
    public void Should_Start_Performing_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Performing);

        operation
            .ActualPeriod
            .Should()
            .Be(new Period(
                start: Mocks.TimeProvider.GetUtcNow(),
                end: null
            ));

        var operationPerformingStartedEvent = (operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationPerformingStarted)
            .Which as OperationPerformingStarted)!;

        operationPerformingStartedEvent
            .PerformingStartDateTime
            .Should()
            .Be(Mocks.TimeProvider.GetUtcNow());
    }

    /// <summary>
    /// Операция должна приостанавливаться корректно.
    /// </summary>
    [Fact]
    public void Should_Pause_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Pause(Mocks.TimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Paused);

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationPaused);
    }

    /// <summary>
    /// Приостановка операции в статусе,
    /// отличающемся от 'Планируется',
    /// должна вернуть ошибку.
    /// </summary>
    [Fact]
    public void Pause_In_Not_Performing_Status_Should_Throw_An_Error()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        var pauseAction =
            () => operation.Pause(Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .Status
            .CannotPauseIfTheStatusIsNotPerforming;
        
        var exception = pauseAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Операция должна быть возобновлена корректно.
    /// </summary>
    [Fact]
    public void Should_Resumed_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Pause(Mocks.TimeProvider);
        operation.Resume(Mocks.TimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Performing);

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationResumed);
    }

    /// <summary>
    /// Возобновление операции в статусе,
    /// отличающемся от 'Приостановлена',
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void Resume_In_Not_Paused_Status_Should_Throw_An_Error()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);

        var resumeAction =
            () => operation.Resume(Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .Status
            .CannotResumeIfTheStatusIsNotPaused;
        
        var exception = resumeAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Операция должна откатиться в статус 'Планируется' корректно.
    /// </summary>
    [Fact]
    public void Should_Rollback_To_Planning_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Rollback(Mocks.TimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Planning);

        operation
            .ActualPeriod
            .Should()
            .Be(null);

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationRollbackToPlanning);
    }

    /// <summary>
    /// Операция должна откатиться в статус 'Выполняется' корректно.
    /// </summary>
    [Fact]
    public void Should_Rollback_To_Performing_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        var completionTimeProvider = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.Now.AddDays(1)
        );

        var rollbackTimeProvider = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.Now.AddDays(2)
        );

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Complete(completionTimeProvider);
        operation.Rollback(rollbackTimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Performing);

        operation
            .ActualPeriod
            .Should()
            .Be(new Period(
                start: Mocks.TimeProvider.GetUtcNow(),
                end: null
            ));

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationRollbackToPerforming);
    }

    /// <summary>
    /// Откат операции в некорректный статус,
    /// должен вернуть ошибку.
    /// </summary>
    [Fact]
    public void Rollback_In_Incorrect_Status_Should_Throw_An_Error()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Pause(Mocks.TimeProvider);

        var rollbackAction =
            () => operation.Rollback(Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .Status
            .CannotRollbackIfTheStatusIsNotPerformingOrCompleted;

        var exception = rollbackAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Операция должна завершиться корректно.
    /// </summary>
    [Fact]
    public void Should_Complete_Correctly()
    {
        // Arrange
        var operation = ArrangeOperation();

        var completionTimeProvider = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.Now.AddDays(1)
        );

        // Act
        operation.Perform(Mocks.TimeProvider);
        operation.Complete(completionTimeProvider);

        // Assert
        operation
            .Status
            .Should()
            .Be(OperationStatus.Completed);

        operation
            .ActualPeriod
            .Should()
            .Be(new Period(
                start: Mocks.TimeProvider.GetUtcNow(),
                end: completionTimeProvider.GetUtcNow()
            ));

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationCompleted);
    }

    /// <summary>
    /// Завершение операции в статусе,
    /// отличающемся от 'Выполняется',
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void Complete_In_Not_Performing_Status_Should_Throw_An_Error()
    {
        // Arrange
        var operation = ArrangeOperation();

        // Act
        var completeAction =
            () => operation.Complete(Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .Status
            .CannotCompleteIfTheStatusIsNotPerforming;

        var exception = completeAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Название операции должно измениться корректно.
    /// </summary>
    [Fact]
    public void Should_Change_A_Name_Correctly()
    {
        // Arrange
        const string newName = "new name";
        var operation = ArrangeOperation();

        // Act
        operation.ChangeName(newName, Mocks.TimeProvider);

        // Assert
        operation
            .Name
            .Should()
            .Be(newName);

        var operationNameChangedEvent = (operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationNameChanged)
            .Which as OperationNameChanged)!;

        operationNameChangedEvent
            .NewName
            .Should()
            .Be(newName);
    }

    /// <summary>
    /// Изменение названия операции в статусе,
    /// отличающемся от 'Планируется',
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void ChangeName_In_Not_Planning_Status_Should_Throw_An_Error()
    {
        // Arrange
        const string newName = "new name";
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);

        var changeNameAction =
            () => operation.ChangeName(newName, Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .CannotChangeANameIfAStatusIsNotPlanning;

        var exception = changeNameAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Описание операции должно измениться корректно.
    /// </summary>
    [Fact]
    public void Should_Change_A_Description_Correctly()
    {
        // Arrange
        const string newDescription = "new description";
        var operation = ArrangeOperation();

        // Act
        operation.ChangeDescription(newDescription, Mocks.TimeProvider);

        // Assert
        operation
            .Description
            .Should()
            .Be(newDescription);

        var operationNameChangedEvent = (operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) => domainEvent is OperationDescriptionChanged)
            .Which as OperationDescriptionChanged)!;

        operationNameChangedEvent
            .NewDescription
            .Should()
            .Be(newDescription);
    }

    /// <summary>
    /// Изменение описания операции в статусе,
    /// отличающемся от 'Планируется',
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void ChangeDescription_In_Not_Planning_Status_Should_Throw_An_Error()
    {
        // Arrange
        const string newDescription = "new name";
        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);

        var changeDescriptionAction =
            () => operation.ChangeDescription(newDescription, Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .CannotChangeADescriptionIfAStatusIsNotPlanning;

        var exception = changeDescriptionAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Плановый период операции должно измениться корректно.
    /// </summary>
    [Fact]
    public void Should_Change_A_Planned_Period_Correctly()
    {
        // Arrange
        var newPlannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddDays(1)
        );

        var operation = ArrangeOperation();

        // Act
        operation.ChangePlannedPeriod(
            newPlannedPeriod,
            Mocks.TimeProvider
        );

        // Assert
        operation
            .PlannedPeriod
            .Should()
            .Be(newPlannedPeriod);

        operation
            .DomainEvents
            .Should()
            .Contain((domainEvent) =>
                domainEvent is OperationPlannedPeriodChanged
            );
    }

    /// <summary>
    /// Изменение планового периода операции в статусе,
    /// отличающемся от 'Планируется',
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void ChangePlannedPeriod_In_Not_Planning_Status_Should_Throw_An_Error()
    {
        // Arrange
        var newPlannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddDays(1)
        );

        var operation = ArrangeOperation();

        // Act
        operation.Perform(Mocks.TimeProvider);

        var changePeriodAction = () =>
            operation.ChangePlannedPeriod(newPlannedPeriod, Mocks.TimeProvider);

        // Assert
        var error = OperationErrors
            .Period
            .CannotChangeAPlannedPeriodBeginningDateTime;

        var exception = changePeriodAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Состояние операции должно быть
    /// сохранено в снэпшот корректно.
    /// </summary>
    [Fact]
    public void Should_Memoize_Correctly()
    {
        // Arrange
        var plannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddHours(1)
        );

        var operation = OperationEntity.Plan(
            OperationName,
            OperationDescription,
            plannedPeriod,
            Mocks.TimeProvider
        );

        // Act
        var snapshot = operation.Memoize();

        // Assert
        snapshot
            .Should()
            .Be(new OperationSnapshot()
            {
                Identifier = operation.Identifier,
                Name = OperationName,
                Description = OperationDescription,
                Status = OperationStatus.Planning,
                PlannedPeriod = plannedPeriod,
                ActualPeriod = null,
            });
    }

    /// <summary>
    /// Состояние операции должно быть
    /// восстановлено из снэпшота корректно.
    /// </summary>
    [Fact]
    public void Should_Restore_Correctly()
    {
        // Arrange
        var identifier = Guid.NewGuid();

        var plannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddHours(1)
        );

        var snapshot = new OperationSnapshot()
        {
            Identifier = identifier,
            Name = OperationName,
            Description = OperationDescription,
            Status = OperationStatus.Planning,
            PlannedPeriod = plannedPeriod,
            ActualPeriod = null,
        };

        // Act
        var restoredOperation =
            OperationEntity.Restore(snapshot);

        // Assert
        restoredOperation
            .Identifier
            .Should()
            .Be(identifier);

        restoredOperation
            .Name
            .Should()
            .Be(OperationName);

        restoredOperation
            .Description
            .Should()
            .Be(OperationDescription);

        restoredOperation
            .Status
            .Should()
            .Be(OperationStatus.Planning);

        restoredOperation
            .PlannedPeriod
            .Should()
            .Be(plannedPeriod);

        restoredOperation
            .ActualPeriod
            .Should()
            .Be(null);
    }

    private static OperationEntity ArrangeOperation()
    {
        var plannedPeriod = new Period(
            start: DateTimeOffset.Now,
            end: DateTimeOffset.Now.AddHours(1)
        );

        var operation = OperationEntity.Plan(
            OperationName,
            OperationDescription,
            plannedPeriod,
            Mocks.TimeProvider
        );

        return operation;
    }
}