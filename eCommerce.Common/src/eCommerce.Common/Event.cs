namespace eCommerce.Common;

/// <summary>
/// Базовый класс для событий.
/// </summary>
public abstract record Event
{
    /// <summary>
    /// Идентификатор события.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Дата создания события.
    /// </summary>
    public required DateTimeOffset DateStamp { get; init; }
}