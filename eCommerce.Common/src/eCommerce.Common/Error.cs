using System.Text;

namespace eCommerce.Common;

/// <summary>
/// Ошибка.
/// </summary>
public class Error
{
    private readonly StringBuilder _messageBuilder;
    private string _currentMessage;

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public ErrorCode Code { get; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Message
    {
        get
        {
            if (_messageBuilder.Equals(_currentMessage))
            {
                return _currentMessage;
            }

            return _currentMessage = _messageBuilder.ToString();
        }
    }

    /// <summary>
    /// Внутренне исключение.
    /// </summary>
    public Exception? InnerException { get; private set; }

    /// <summary>
    /// Создать ошибку на основе ее кода и сообщения.
    /// </summary>
    /// <param name="code">Код ошибки.</param>
    /// <param name="message">Сообщение об ошибке.</param>
    public Error(ErrorCode code, string message)
    {
        _messageBuilder = new StringBuilder(message);
        _currentMessage = message;

        Code = code;
    }

    /// <summary>
    /// Добавить префикс к сообщению.
    /// </summary>
    /// <param name="messagePrefix">Префикс сообщения.</param>
    /// <returns>
    /// Ошибку с добавленным префиксом в сообщении.
    /// </returns>
    public Error AppendMessagePrefix(string messagePrefix)
    {
        _messageBuilder.Insert(0, ' ');
        _messageBuilder.Insert(0, messagePrefix);

        return this;
    }

    /// <summary>
    /// Добавить в конец сообщения дополнительное сообщение.
    /// </summary>
    /// <param name="message">Дополнительное сообщение.</param>
    /// <returns>
    /// Ошибку с добавленным к ее сообщению дополнительным сообщением.
    /// </returns>
    public Error AppendMessage(string message)
    {
        _messageBuilder.Append(' ');
        _messageBuilder.Append(message);

        return this;
    }

    /// <summary>
    /// Добавить к ошибке внутреннее исключение.
    /// </summary>
    /// <param name="innerException">Внутреннее исключение.</param>
    /// <returns>
    /// Ошибку с добавленным к ней внутренним исключением.
    /// </returns>
    public Error WithInnerException(Exception innerException)
    {
        InnerException = innerException;

        return this;
    }

    /// <summary>
    /// Преобразовать ошибку в исключение.
    /// </summary>
    /// <returns>
    /// Исключение, созданное на основе ошибки.
    /// </returns>
    public ErrorException ToException()
    {
        return new ErrorException(
            Code,
            Message,
            InnerException
        );
    }

    /// <summary>
    /// Преобразовать ошибку в исключение.
    /// </summary>
    /// <param name="error">Ошибка, которую нужно преобразовать в исключение.</param>
    /// <returns>
    /// Исключение, созданное на основе ошибки.
    /// </returns>
    public static implicit operator ErrorException(Error error) =>
        error.ToException();
}