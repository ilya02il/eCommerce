namespace eCommerce.Common;

/// <summary>
/// Исключение, причиной которого стала ошибка.
/// </summary>
public sealed class ErrorException : Exception
{
    /// <summary>
    /// Код ошибки.
    /// </summary>
    public ErrorCode ErrorCode { get; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Создать новое исключение на
    /// основе информации об ошибке.
    /// </summary>
    /// <param name="errorCode">
    /// Код ошибки.
    /// </param>
    /// <param name="message">
    /// Сообщение об ошибке.
    /// </param>
    /// <param name="innerException">
    /// Внутреннее исключение.
    /// </param>
    internal ErrorException(
        ErrorCode errorCode,
        string message,
        Exception? innerException
    )
        : base(
            CreateExceptionMessage(errorCode, message),
            innerException
        )
    {
        ErrorCode = errorCode;
        ErrorMessage = message;
    }

    private static string CreateExceptionMessage(
        ErrorCode errorCode,
        string message
    )
    {
        return $"{errorCode}: {message}";
    }
}