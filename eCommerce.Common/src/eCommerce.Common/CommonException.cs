using System.Text.RegularExpressions;

namespace eCommerce.Common;

/// <summary>
/// Исключение уровня базовой библиотеки.
/// </summary>
internal partial class CommonException : BaseException
{
    [GeneratedRegex(@"^COMMON-[A-ZА-Я-]{1,}[-]?(?>\d{1,})?$", RegexOptions.Compiled)]
    private static partial Regex GetCoreErrorCodeRegex();

    /// <summary>
    /// Регулярное выражения для строки кода ошибки.
    /// </summary>
    protected override Regex ErrorCodeRegex => GetCoreErrorCodeRegex();

    /// <inheritdoc/>
    public CommonException(string errorCode, string message)
        : base(errorCode, message)
    {
    }

    /// <inheritdoc/>
    public CommonException(string errorCode, Exception referencedException)
        : base(errorCode, referencedException)
    {
    }
}