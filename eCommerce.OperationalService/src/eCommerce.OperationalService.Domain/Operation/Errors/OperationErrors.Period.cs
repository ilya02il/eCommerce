using eCommerce.Common;

namespace eCommerce.OperationalService.Domain.Operation.Errors;

/// <summary>
/// Ошибки, возникающие при работе с операцией.
/// </summary>
public static partial class OperationErrors
{
    /// <summary>
    /// Ошибки, возникающие при работе с периодами операции.
    /// </summary>
    public static class Period
    {
        /// <summary>
        /// Невозможно изменить дату и время начала планового периода операции.
        /// Дату и время начала планового периода можно изменить только в статусе 'Планируется'.
        /// </summary>
        public static readonly Error CannotChangeAPlannedPeriodBeginningDateTime = new(
            code: "OPER-ROOT-PERIOD-001",
            message:
                "Невозможно изменить дату и время начала планового периода операции. " +
                "Дату и время начала планового периода можно изменить только в статусе 'Планируется'."
        );
    }
}