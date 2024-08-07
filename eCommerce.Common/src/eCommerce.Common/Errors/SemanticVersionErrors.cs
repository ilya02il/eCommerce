﻿namespace eCommerce.Common.Errors;

/// <summary>
/// Ошибки, возникающие в <see cref="SemanticVersion"/>.
/// </summary>
public static class SemanticVersionErrors
{
    /// <summary>
    /// Некорректный формат постфикса версии.
    /// Постфикс может содержать только латинские буквы,
    /// цифры и следующие знаки: '-', '.'.
    /// </summary>
    public static readonly Error IncorrectPostfixFormat = new(
        code: "COMMON-VERSION-001",
        message:
            "Некорректный формат постфикса версии. " +
            "Постфикс может содержать только латинские буквы, " +
            "цифры и следующие знаки: '-', '.'."
    );
}