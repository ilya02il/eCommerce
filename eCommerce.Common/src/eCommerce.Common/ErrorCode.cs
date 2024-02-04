using System.Text.RegularExpressions;

namespace eCommerce.Common;

/// <summary>
/// Структура кода ошибки.
/// </summary>
public readonly partial struct ErrorCode
{
    private readonly string _errorCode;

    /// <summary>
    /// Создать новый экземпляр кода ошибки,
    /// используя строку кода ошибки и регулярное выражение для нее.
    /// </summary>
    /// <param name="errorCode">
    /// Строка кода ошибки.
    /// </param>
    /// <param name="errorCodeRegex">
    /// Регулярное выражение для строки кода ошибки.
    /// </param>
    /// <exception cref="CommonException">
    /// Исключение, которое будет выброшено если
    /// строка кода ошибки не соответствует регулярному выражению.
    /// </exception>
    public ErrorCode(string errorCode, Regex errorCodeRegex)
    {
        var errorCodeMatch = errorCodeRegex.Match(errorCode);

        if (errorCodeMatch.Success is false)
        {
            throw new CommonException(
                errorCode: "COMMON-ERRCODE-001",
                message:
                    "Некорректный формат строки кода ошибки. " +
                    "Строка кода ошибки должна соответствовать следующему регулярному выражению: " +
                    $"'{errorCodeRegex}'."
            );
        }

        _errorCode = errorCode;
    }

    /// <summary>
    /// Получить код ошибки в виде строки.
    /// </summary>
    /// <returns>
    /// Код ошибки в виде строки.
    /// </returns>
    public override string ToString() => _errorCode;

    /// <inheritdoc/>
    public static implicit operator string(ErrorCode errorCode) => errorCode._errorCode;

    /// <summary>
    /// Проверить равны ли два два объекта как коды ошибок.
    /// </summary>
    /// <param name="obj">
    /// Другой объект.
    /// </param>
    /// <returns>
    /// <see langword="true"/> -
    /// если объекты имеют одинаковый тип и их строки кодов ошибок равны,
    /// также если другой объект - строка, которая равна строке текущего кода ошибки,
    /// иначе  <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is string otherErrorCodeString)
        {
            return _errorCode.Equals(otherErrorCodeString);
        }

        if (obj is not ErrorCode otherErrorCode)
        {
            return false;
        }

        return _errorCode.Equals(otherErrorCode._errorCode);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _errorCode.GetHashCode();
    }

    /// <inheritdoc/>
    public static bool operator ==(ErrorCode left, ErrorCode right)
    {
        return left.Equals(right) is true;
    }

    /// <inheritdoc/>
    public static bool operator !=(ErrorCode left, ErrorCode right)
    {
        return left.Equals(right) is false;
    }
}