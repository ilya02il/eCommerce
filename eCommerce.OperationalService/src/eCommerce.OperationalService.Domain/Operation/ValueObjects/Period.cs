using eCommerce.OperationalService.Domain.Operation.Errors;

using Version = eCommerce.Common.Version;

namespace eCommerce.OperationalService.Domain.Operation.ValueObjects;

/// <summary>
/// Объект-значение периода операции.
/// </summary>
public record Period : ValueObject
{
    /// <inheritdoc />
    public override string AggregateName => nameof(Operation);

    /// <inheritdoc />
    public override Version AggregateVersion => new(major: 1);

    /// <summary>
    /// Дата и время начала периода.
    /// </summary>
    public DateTimeOffset? Start { get; private init; }

    /// <summary>
    /// Дата и время окончания периода.
    /// </summary>
    public DateTimeOffset? End { get; private init; }

    /// <summary>
    /// Создать новый период на основе даты и времени начала и окончания.
    /// </summary>
    /// <param name="start">
    /// Дата и время начала.
    /// </param>
    /// <param name="end">
    /// Дата и время окончания.
    /// </param>
    /// <exception cref="PeriodErrors.CannotCreateAnEmptyPeriod"/>
    /// <exception cref="PeriodErrors.CannotCreateWithStartMoreThanEnd"/>
    /// <exception cref="PeriodErrors.CannotCreateWithEndLessThanStart"/>
    public Period(DateTimeOffset? start, DateTimeOffset? end)
    {
        if ((start, end) is (null, null))
        {
            throw PeriodErrors
                .CannotCreateAnEmptyPeriod;
        }
        
        if (start >= end)
        {
            throw PeriodErrors
                .CannotCreateWithStartMoreThanEnd;
        }

        Start = start;
        End = end;
    }

    /// <summary>
    /// Проверить накладываются ли
    /// данный период операции с другим.
    /// </summary>
    /// <param name="otherPeriod">
    /// Другой период операции.
    /// </param>
    /// <returns>
    /// <see langword="true"/> - если периоды накладываются,
    /// иначе - <see langword="false"/>.
    /// </returns>
    public bool IsOverlap(Period otherPeriod)
    {
        return
            Start < otherPeriod.End &&
            End > otherPeriod.Start;
    }
}