namespace eCommerce.Common.Errors;

/// <summary>
/// Ошибки, возникающие в <see cref="ErrorCode"/>.
/// </summary>
public static class ErrorCodeErrors
{
    /// <summary>
    /// Некорректный формат строки кода ошибки.
    /// </summary>
    public static readonly Error AnInputStringDoesNotMatchThePattern = new(
        code: "COMMON-ERR-CODE-001",
        message: "Некорректный формат строки кода ошибки."
    );
}