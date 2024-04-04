using System.Text.RegularExpressions;

using eCommerce.Common.Errors;

namespace eCommerce.Common;

/// <summary>
/// Структура кода ошибки.
/// </summary>
public readonly partial struct ErrorCode
{
    [GeneratedRegex(@"^[A-Z]{1,}(?>\-[A-Z]{1,}){0,}-\d{3}$")]
    private static partial Regex GetErrorCodeRegex();

    private readonly string _errorCode;

    /// <summary>
    /// Создать новый экземпляр кода ошибки,
    /// используя строку кода ошибки и регулярное выражение для нее.
    /// </summary>
    /// <param name="errorCode">
    /// Строка кода ошибки.
    /// </param>
    public ErrorCode(string errorCode)
    {
        if (GetErrorCodeRegex().IsMatch(errorCode) is false)
        {
            throw ErrorCodeErrors
                .AnInputStringDoesNotMatchThePattern
                .AppendMessage(
                    "Строка кода ошибки должна соответствовать " +
                    "следующему регулярному выражению: " +
                    $"'{GetErrorCodeRegex()}'."
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

    /// <summary>
    /// Неявное преобразование строки в код ошибки.
    /// </summary>
    /// <param name="errorCode">
    /// Строка, которую необходимо преобразовать в код ошибки.
    /// </param>
    /// <returns>
    /// Код ошибки, получившийся в результате преобразования строки.
    /// </returns>
    public static implicit operator ErrorCode(string errorCode) =>
        new(errorCode);

    /// <summary>
    /// Неявное преобразование кода ошибки к строке.
    /// </summary>
    /// <param name="errorCode">
    /// Код ошибки, который необходимо преобразовать.
    /// </param>
    /// <returns>
    /// Преобразованный в строку код ошибки.
    /// </returns>
    public static implicit operator string(ErrorCode errorCode) =>
        errorCode._errorCode;

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

        return
            obj is ErrorCode otherErrorCode &&
            _errorCode.Equals(otherErrorCode._errorCode);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _errorCode.GetHashCode();
    }

    /// <summary>
    /// Проверяет на равенство два кода ошибки с
    /// помощью метода <see cref="Equals"/>
    /// и возвращает результат его выполнения.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>
    /// Результат выполнения проверки на равенство.
    /// </returns>
    public static bool operator ==(ErrorCode left, ErrorCode right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Проверяет на неравенство два кода ошибки с
    /// помощью метода <see cref="Equals"/> и возвращает результат,
    /// обратный результату этого метода.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>
    /// Результат выполнения проверки на неравенство.
    /// </returns>
    public static bool operator !=(ErrorCode left, ErrorCode right)
    {
        return !(left == right);
    }
}