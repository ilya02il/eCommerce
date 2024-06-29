using System.Text.RegularExpressions;

using eCommerce.Common.Errors;

namespace eCommerce.Common;

/// <summary>
/// Семантическая версия чего-либо.
/// </summary>
public readonly partial struct SemanticVersion
{
    [GeneratedRegex(@"^[A-Za-z\d\-.]*$", RegexOptions.Compiled)]
    private static partial Regex GetPostfixRegex();

    private readonly string? _postfix = null;

    /// <summary>
    /// Мажорная версия. Поднимается если были сделаны
    /// глобальные обратно-несовместимые изменения.
    /// </summary>
    public uint Major { get; } = 1;

    /// <summary>
    /// Минорная версия. Поднимается если были сделаны
    /// небольшие обратно-совместимые изменения.
    /// </summary>
    public uint Minor { get; } = 0;

    /// <summary>
    /// Патч-версия. Поднимается если были
    /// исправлены какие-либо баги.
    /// </summary>
    public uint Patch { get; } = 0;

    /// <summary>
    /// Билд-версия. Поднимается после каждого билда
    /// новой версии при разработке.
    /// </summary>
    public uint Build { get; } = 0;

    /// <summary>
    /// Постфикс версии. Добавляет описание к версии в конце,
    /// например о том, что данная версия является предрелизной.
    /// </summary>
    public string? Postfix
    {
        get => _postfix;
        private init
        {
            if (value is not null && GetPostfixRegex().IsMatch(value) is false)
            {
                throw SemanticVersionErrors.IncorrectPostfixFormat;
            }

            _postfix = value;
        }
    }

    /// <summary>
    ///     Создать новый экземпляр семантической версии.
    /// </summary>
    /// <param name="major">
    ///     Мажорная версия. Поднимается если были сделаны глобальные обратно-несовместимые изменения.
    /// </param>
    /// <param name="minor">
    ///     Минорная версия. Поднимается если были сделаны небольшие обратно-совместимые изменения.
    /// </param>
    /// <param name="patch">
    ///     Патч-версия. Поднимается если были исправлены какие-либо баги.
    /// </param>
    /// <param name="build">
    ///     Билд-версия. Поднимается после каждого билда новой версии при разработке.
    /// </param>
    /// <param name="postfix">
    ///     Постфикс версии. Добавляет описание к версии в конце,
    ///     например о том, что данная версия является предрелизной.
    /// </param>
    public SemanticVersion(
        uint major,
        uint minor = 0,
        uint patch = 0,
        uint build = 0,
        string? postfix = null
    )
    {
        Major = major;
        Minor = minor;
        Patch = patch;
        Build = build;
        Postfix = postfix;
    }

    /// <summary>
    ///     Проверить версию на совместимость с другой версией.
    /// </summary>
    /// <param name="otherVersion">
    ///     Версия, с которой проверяется совместимость.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> - если версии совместимы, иначе <see langword="false"/>.
    /// </returns>
    public bool IsCompatibleWith(SemanticVersion otherVersion)
    {
        return otherVersion.Major == Major;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is SemanticVersion other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode =
            Major ^ Minor ^ Patch ^ Build ^
            (Postfix?.GetHashCode() ?? 0);

        return unchecked((int)hashCode);
    }

    /// <summary>
    ///     Сравнить две семантические версии.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>
    ///     <see langword="true"/> - если все компоненты версии <paramref name="left"/>
    ///     равны всем компонентам версии <paramref name="right"/>, иначе - <see langword="false"/>.
    /// </returns>
    public static bool operator ==(SemanticVersion left, SemanticVersion right)
    {
        return left.Equals(right) is true;
    }

    /// <summary>
    ///     Сравнить две семантические версии.
    /// </summary>
    /// <param name="left">Левый операнд.</param>
    /// <param name="right">Правый операнд.</param>
    /// <returns>
    ///     <see langword="false"/> - если хотя бы один компонент версии <paramref name="left"/>
    ///     не равен соответствующему компоненту версии <paramref name="right"/>, иначе - <see langword="true"/>.
    /// </returns>
    public static bool operator !=(SemanticVersion left, SemanticVersion right)
    {
        return left.Equals(right) is false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var stringifiedVersion =
            string.Join(".", Major, Minor, Patch, Build);

        if (string.IsNullOrEmpty(Postfix))
        {
            return stringifiedVersion;
        }

        return $"{stringifiedVersion}-{Postfix}";
    }

    private bool Equals(SemanticVersion otherVersion)
    {
        return
            Major == otherVersion.Major &&
            Minor == otherVersion.Minor &&
            Patch == otherVersion.Patch &&
            Build == otherVersion.Build &&
            Postfix == otherVersion.Postfix;
    }
}