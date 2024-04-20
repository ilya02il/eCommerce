namespace eCommerce.OperationalService.Domain.Operation.Enums;

/// <summary>
/// Перечисление со статусами операции.
/// </summary>
public enum OperationStatus : byte
{
    /// <summary>
    /// Планируется.
    /// </summary>
    Planning = 1,
    
    /// <summary>
    /// Выполняется.
    /// </summary>
    Performing = 2,

    /// <summary>
    /// Приостановлена.
    /// </summary>
    Paused = 3,

    /// <summary>
    /// Завершена.
    /// </summary>
    Completed = 4
}