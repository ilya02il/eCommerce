using eCommerce.Common;
using eCommerce.OperationalService.Domain.Operation.Errors;
using eCommerce.OperationalService.Domain.Operation.ValueObjects;

namespace eCommerce.OperationalService.Tests.Operation.ValueObjects;

/// <summary>
/// Класс с тестами объекта-значения периода.
/// </summary>
public class PeriodTests
{
    /// <summary>
    /// Объект-значение периода должен иметь правильные
    /// название агрегата и его версию.
    /// </summary>
    [Fact]
    public void Should_Have_Correct_Aggregate_Name_And_Version()
    {
        // Arrange
        var endTimeProvider = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.Now.AddDays(1)
        );

        var period = new Period(
            start: Mocks.TimeProvider.GetUtcNow(),
            end: endTimeProvider.GetUtcNow()
        );

        // Assert
        period
            .AggregateName
            .Should()
            .Be(nameof(Operation));

        period
            .AggregateVersion
            .Major
            .Should()
            .Be(1);
    }

    /// <summary>
    /// Период должен быть создан корректно.
    /// </summary>
    [Fact]
    public void Should_Creates_Correctly()
    {
        // Arrange
        var endTimeProvider = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.Now.AddDays(1)
        );

        var period = new Period(
            start: Mocks.TimeProvider.GetUtcNow(),
            end: endTimeProvider.GetUtcNow()
        );

        // Assert
        period
            .Start
            .Should()
            .Be(Mocks.TimeProvider.GetUtcNow());

        period
            .End
            .Should()
            .Be(endTimeProvider.GetUtcNow());
    }

    /// <summary>
    /// Создание пустого периода должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void Create_An_Empty_Period_Should_Throw_An_Error()
    {
        // Act
        var creationAction =
            () => new Period(start: null, end: null);

        // Assert
        var error = PeriodErrors
            .CannotCreateAnEmptyPeriod;

        var exception = creationAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Создание периода с датой и временем начала,
    /// превышающими дату и время окончания,
    /// должно вернуть ошибку.
    /// </summary>
    [Fact]
    public void Create_With_Start_More_Than_End_Should_Throw_An_Error()
    {
        // Act
        var creationAction = () => new Period(
            start: Mocks.TimeProvider.GetUtcNow().AddDays(1),
            end: Mocks.TimeProvider.GetUtcNow()
        );

        // Assert
        var error = PeriodErrors
            .CannotCreateWithStartMoreThanEnd;

        var exception = creationAction
            .Should()
            .Throw<ErrorException>()
            .Which;

        exception.AssertByError(error);
    }

    /// <summary>
    /// Проверка на пересечение периодов должна вернуть <see langword="true"/>.
    /// </summary>
    [Fact]
    public void Periods_Should_Overlap()
    {
        // Arrange
        var firstPeriod = new Period(
            start: Mocks.TimeProvider.GetUtcNow(),
            end: Mocks.TimeProvider.GetUtcNow().AddDays(7)
        );

        var secondPeriod = new Period(
            start: Mocks.TimeProvider.GetUtcNow().AddDays(3),
            end: Mocks.TimeProvider.GetUtcNow().AddDays(12)
        );

        // Act
        var isOverlap =
            firstPeriod.IsOverlap(secondPeriod);

        // Assert
        isOverlap
            .Should()
            .BeTrue();
    }

    /// <summary>
    /// Проверка на пересечение периодов должна вернуть <see langword="false"/>.
    /// </summary>
    [Fact]
    public void Periods_Should_Not_Overlap()
    {
        // Arrange
        var firstPeriod = new Period(
            start: Mocks.TimeProvider.GetUtcNow(),
            end: Mocks.TimeProvider.GetUtcNow().AddDays(7)
        );

        var secondPeriod = new Period(
            start: Mocks.TimeProvider.GetUtcNow().AddDays(7),
            end: Mocks.TimeProvider.GetUtcNow().AddDays(12)
        );

        // Act
        var isOverlap =
            firstPeriod.IsOverlap(secondPeriod);

        // Assert
        isOverlap
            .Should()
            .BeFalse();
    }
}