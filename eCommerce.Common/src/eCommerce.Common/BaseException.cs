using System.Text.RegularExpressions;

namespace eCommerce.Common;

/// <summary>
/// Базовый класс для всех исключений в проектах
/// (использовать как базовый класс вместо <see cref="Exception"/>).
/// </summary>
public abstract partial class BaseException : Exception
{
    [GeneratedRegex(@"^[A-ZА-Я]{1,}-\d{1,}$", RegexOptions.Compiled)]
    private static partial Regex GetDefaultErrorCodeRegex();

    private static string GetFormattedErrorMessage(string errorCode, string errorMessage)
    {
        return $"{errorCode}: {errorMessage}";
    }

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public ErrorCode ErrorCode { get; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Регулярное выражения для строки кода ошибки.
    /// </summary>
    protected virtual Regex ErrorCodeRegex => GetDefaultErrorCodeRegex();

    /// <summary>
    /// Создать новый экземпляр исключения,
    /// используя код ошибки и другое исключение.
    /// </summary>
    /// <param name="errorCode">
    /// Код ошибки.
    /// </param>
    /// <param name="referencedException">
    /// Другое исключение, сообщение которого будет
    /// взято за основу для создаваемого исключения.
    /// </param>
    protected BaseException(string errorCode, Exception referencedException)
        : base(GetFormattedErrorMessage(errorCode, referencedException.Message))
    {
        ErrorCode = new ErrorCode(errorCode, ErrorCodeRegex);
        ErrorMessage = referencedException.Message;
    }

    /// <summary>
    /// Создать новый экземпляр исключения,
    /// используя код ошибки и сообщение об ошибке.
    /// </summary>
    /// <param name="errorCode">
    /// Код ошибки.
    /// </param>
    /// <param name="errorMessage">
    /// Сообщение об ошибке.
    /// </param>
    protected BaseException(string errorCode, string errorMessage)
        : base(GetFormattedErrorMessage(errorCode, errorMessage))
    {
        ErrorCode = new ErrorCode(errorCode, ErrorCodeRegex);
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Создать новый экземпляр исключения,
    /// используя код ошибки, сообщение об ошибке и
    /// исключение, которое станет вложенным для создаваемого.
    /// </summary>
    /// <param name="errorCode">
    /// Код ошибки.
    /// </param>
    /// <param name="errorMessage">
    /// Сообщение об ошибке.
    /// </param>
    /// <param name="innerException">
    /// Иключение, которое станет вложенным для создаваемого.
    /// </param>
    protected BaseException(
        string errorCode,
        string errorMessage,
        Exception innerException
    )
    : base(
        GetFormattedErrorMessage(errorCode, errorMessage),
        innerException
    )
    {
        ErrorCode = new ErrorCode(errorCode, ErrorCodeRegex);
        ErrorMessage = errorMessage;
    }
}
