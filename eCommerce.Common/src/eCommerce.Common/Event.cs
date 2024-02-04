namespace eCommerce.Common;

/// <summary>
/// Базовый класс для событий.
/// </summary>
public abstract record Event
{
    /// <summary>
    /// Создать событие,
    /// в качестве провайдера для инциализации <see cref="DateStamp"/>
    /// которого будет использоваться стандартный провайдер
    /// (<see cref="TimeProvider.System"/>).
    /// </summary>
    protected Event()
    {
        DateStamp = TimeProvider.System.GetUtcNow();
    }

    /// <summary>
    /// Создать событие,
    /// в качестве провайдера для инциализации <see cref="DateStamp"/>
    /// которого будет использоваться <paramref name="timeProvider"/>.
    /// </summary>
    /// <param name="timeProvider">
    /// Провайдер для инициализации <see cref="DateStamp"/>.
    /// </param>
    protected Event(TimeProvider timeProvider)
    {
        DateStamp = timeProvider.GetUtcNow();
    }

    /// <summary>
    /// Идентификатор события.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Дата создания события.
    /// </summary>
    public DateTimeOffset DateStamp { get; }
}