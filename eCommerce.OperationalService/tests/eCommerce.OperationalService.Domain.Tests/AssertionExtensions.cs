using eCommerce.Common;

namespace eCommerce.OperationalService.Tests;

/// <summary>
/// Класс с методами расширения для проверки утверждений.
/// </summary>
internal static class AssertionExtensions
{
    /// <summary>
    /// Проверить исключение, возникшее в результате ошибки,
    /// на соответствие ошибке.
    /// </summary>
    /// <param name="exception">
    /// Исключение, возникшее в результате ошибки.
    /// </param>
    /// <param name="error">Ошибка.</param>
    public static void AssertByError(
        this ErrorException exception,
        Error error
    )
    {
        exception
            .ErrorCode
            .Should()
            .Be(error.Code);

        exception
            .ErrorMessage
            .Should()
            .Contain(error.Message);
    }
}