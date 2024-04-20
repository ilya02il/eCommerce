using eCommerce.Common;

namespace eCommerce.OperationalService.Domain.Operation.Errors;

/// <summary>
/// Ошибки, возникающие при работе с операцией.
/// </summary>
public static partial class OperationErrors
{
    /// <summary>
    /// Ошибки, возникающие при работе со статусом операции.
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// Выполнение операции можно приостановить только в статусе 'Выполняется'.
        /// </summary>
        public static readonly Error CannotPauseIfTheStatusIsNotPerforming = new(
            code: "OPER-ROOT-STATUS-001",
            message: "Выполнение операции можно приостановить только в статусе 'Выполняется'."
        );

        /// <summary>
        /// Возобновить выполнение оперции можно только из статуса 'Приостановлена'.
        /// </summary>
        public static readonly Error CannotResumeIfTheStatusIsNotPaused = new(
            code: "OPER-ROOT-STATUS-002",
            message: "Возобновить выполнение оперции можно только из статуса 'Приостановлена'."
        );

        /// <summary>
        /// Откатить операцию в предыдущий статус можно только в
        /// статусах 'Выполняется' и 'Завершена'.
        /// </summary>
        public static readonly Error CannotRollbackIfTheStatusIsNotPerformingOrCompleted = new(
            code: "OPER-ROOT-STATUS-003",
            message:
            "Откатить операцию в предыдущий статус можно только в" +
            "статусах 'Выполняется' и 'Завершена'."
        );

        /// <summary>
        /// Завершить оперцию можно только из статуса 'Выполняется'.
        /// </summary>
        public static readonly Error CannotCompleteIfTheStatusIsNotPerforming = new(
            code: "OPER-ROOT-STATUS-004",
            message: "Завершить оперцию можно только из статуса 'Выполняется'."
        );
    }
}