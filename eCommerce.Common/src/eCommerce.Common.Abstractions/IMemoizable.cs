namespace eCommerce.Common.Abstractions;

/// <summary>
/// Интерфейс мемоизируемого объекта,
/// состояние которого можно сохранить в некий объект (мементо),
/// а также восстановить его обратно из этого объекта.
/// </summary>
/// <typeparam name="TMemoizable">
/// Тип мемоизируемого объекта.
/// </typeparam>
/// <typeparam name="TMemento">
/// Тип объекта, в который будет сохраняться
/// состояние мемоизируемого объекта, (мементо).
/// </typeparam>
public interface IMemoizable<out TMemoizable, TMemento>
{
    /// <summary>
    /// Восстановить объект вместе с его состоянием из мементо.
    /// </summary>
    /// <param name="memento">
    /// Объект, из которого будет восстановлено
    /// состояние мемоизируемого объекта.
    /// </param>
    /// <returns>
    /// Объект, восстановленный из мементо.
    /// </returns>
    public static abstract TMemoizable Restore(TMemento memento);

    /// <summary>
    /// Сохранить состояние объекта в мементо.
    /// </summary>
    /// <returns>
    /// Объект, в который было сохранено
    /// состояние мемоизируемого объекта.
    /// </returns>
    public TMemento Memoize();
}
