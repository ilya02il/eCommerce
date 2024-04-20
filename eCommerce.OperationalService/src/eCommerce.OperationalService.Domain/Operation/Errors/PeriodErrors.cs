using eCommerce.Common;

namespace eCommerce.OperationalService.Domain.Operation.Errors;

/// <summary>
/// Ошибки, возникающие в объекте-значения периода.
/// </summary>
public static class PeriodErrors
{
    /// <summary>
    /// Период не может не иметь даты и времени
    /// начала и окончания одновременно.
    /// </summary>
    public static readonly Error CannotCreateAnEmptyPeriod = new(
        code: "OPER-PERIOD-001",
        message:
            "Период не может не иметь даты и времени " +
            "начала и окончания одновременно."
    );

    /// <summary>
    /// Дата и время начала периода не могут быть
    /// больше или равны дате и времени его окончания.
    /// </summary>
    public static readonly Error CannotCreateWithStartMoreThanEnd = new(
        code: "OPER-PERIOD-002",
        message:
            "Дата и время начала периода не могут быть " +
            "больше или равны дате и времени его окончания."
    );
}