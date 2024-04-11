namespace eCommerce.Common.Domain;

/// <summary>
/// Интерфейс члена агрегата.
/// </summary>
public interface IAggregateMember
{
    /// <summary>
    /// Название агрегата.
    /// </summary>
    public string AggregateName { get; }

    /// <summary>
    /// Семантическая версия агрегата.
    /// </summary>
    public Version AggregateVersion { get; }
}