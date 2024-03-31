using System.Text.RegularExpressions;

namespace eCommerce.Common;

/// <summary>
/// Семантическая версия чего-либо.
/// </summary>
public sealed partial record Version
{
    [GeneratedRegex(@"^[A-Za-z\d\-.]*$", RegexOptions.Compiled)]
    private static partial Regex GetPostfixRegex();

    private readonly string? _postfix;

    /// <summary>
    /// Мажорная версия. Поднимается если были сделаны
    /// глобальные обратно-несовместимые изменения.
    /// </summary>
    public uint Major { get; private init; }

    /// <summary>
    /// Минорная версия. Поднимается если были сделаны
    /// небольшие обратно-совместимые изменения.
    /// </summary>
    public uint Minor { get; private init; }

    /// <summary>
    /// Патч-версия. Поднимается если были
    /// исправлены какие-либо баги.
    /// </summary>
    public uint Patch { get; private init; }

    /// <summary>
    /// Билд-версия. Поднимается после каждого билда
    /// новой версии при разработке.
    /// </summary>
    public uint Build { get; private init; }

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
                throw new CommonException(
                    errorCode: "COMMON-VERSION-001",
                    message:
                        "Некорректный формат постфикса версии. " +
                        "Постфикс может содержать только латинские буквы, " +
                        "цифры и следующие знаки: '-', '.'."
                );
            }

            _postfix = value;
        }
    }

    /// <summary>
    /// Создать новый экземпляр семантической версии.
    /// </summary>
    /// <param name="major">
    /// Мажорная версия. Поднимается если были сделаны
    /// глобальные обратно-несовместимые изменения.
    /// </param>
    /// <param name="minor">
    /// Минорная версия. Поднимается если были сделаны
    /// небольшие обратно-совместимые изменения.
    /// </param>
    /// <param name="patch">
    /// Патч-версия. Поднимается если были
    /// исправлены какие-либо баги.
    /// </param>
    /// <param name="build">
    /// Билд-версия. Поднимается после каждого билда
    /// новой версии при разработке.
    /// </param>
    /// <param name="postfix">
    /// Постфикс версии. Добавляет описание к версии в конце,
    /// например о том, что данная версия является предрелизной.
    /// </param>
    public Version(
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
    /// Проверить версию на совместимость с другой версией.
    /// </summary>
    /// <param name="otherVersion">
    /// Версия, с которой проверяется совместимость.
    /// </param>
    /// <returns>
    /// <see langword="true"/> - если версии совместимы,
    /// иначе <see langword="false"/>.
    /// </returns>
    public bool IsCompatibleWith(Version otherVersion)
    {
        return otherVersion.Major == Major;
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
}