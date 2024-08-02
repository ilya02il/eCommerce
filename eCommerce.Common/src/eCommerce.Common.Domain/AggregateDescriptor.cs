namespace eCommerce.Common.Domain;

/// <summary>
/// Дескриптор агрегата.
/// </summary>
public readonly struct AggregateDescriptor
{
    /// <summary>
    /// Название агрегата.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Семантическая версия агрегата.
    /// </summary>
    public required SemanticVersion Version { get; init; }
}