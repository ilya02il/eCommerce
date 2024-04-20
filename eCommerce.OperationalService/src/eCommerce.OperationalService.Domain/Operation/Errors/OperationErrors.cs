using eCommerce.Common;

namespace eCommerce.OperationalService.Domain.Operation.Errors;

/// <summary>
/// Ошибки, возникающие при работе с операцией.
/// </summary>
public static partial class OperationErrors
{
    /// <summary>
    /// Изменить название можно только у операции в статусе 'Планируется'.
    /// </summary>
    public static readonly Error CannotChangeANameIfAStatusIsNotPlanning = new(
        code: "OPER-ROOT-001",
        message:
        "Изменить название можно только у операции в статусе 'Планируется'."
    );

    /// <summary>
    /// Изменить описание можно только у операции в статусе 'Планируется'.
    /// </summary>
    public static readonly Error CannotChangeADescriptionIfAStatusIsNotPlanning = new(
        code: "OPER-ROOT-002",
        message:
        "Изменить описание можно только у операции в статусе 'Планируется'."
    );
}