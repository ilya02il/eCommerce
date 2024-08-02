namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс для корня агрегата.
/// </summary>
/// <typeparam name="TId">Тип идентификатора корня агрегата.</typeparam>
/// <param name="identifier">Идентификатор корня агрегата.</param>
public abstract class AggregateRoot<TId>(TId identifier) : Entity<TId>(identifier)
    where TId : IEquatable<TId>;