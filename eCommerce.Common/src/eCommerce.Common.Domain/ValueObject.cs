namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс для объектов-значений.
/// </summary>
public abstract record ValueObject : IAggregateMember
{
    /// <inheritdoc />
    public abstract string AggregateName { get; }

    /// <inheritdoc />
    public abstract Version AggregateVersion { get; }
}