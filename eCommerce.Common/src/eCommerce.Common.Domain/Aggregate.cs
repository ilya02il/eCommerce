namespace eCommerce.Common.Domain;

/// <summary>
/// Агрегат.
/// </summary>
/// <typeparam name="TRoot">Тип корня агрегата.</typeparam>
/// <typeparam name="TRootId">Тип идентификатора корня агрегата.</typeparam>
public class Aggregate<TRoot, TRootId>
    where TRoot : AggregateRoot<TRootId>
    where TRootId : IEquatable<TRootId>
{
    /// <summary>
    /// Дескриптор агрегата.
    /// </summary>
    public AggregateDescriptor Descriptor { get; }

    /// <summary>
    /// Корень агрегата.
    /// </summary>
    public TRoot Root { get; }

    /// <summary>
    /// Коллекция незафиксированных событий предметной области.
    /// </summary>
    public Queue<DomainEvent<TRootId>> UncommittedEvents { get; } = [];

    /// <summary>
    /// Создать агрегат с указанным дескриптором,
    /// корнем агрегации и словарем с мутациями агрегата.
    /// </summary>
    /// <param name="descriptor">Дескриптор.</param>
    /// <param name="root">Корень агрегата.</param>
    public Aggregate(AggregateDescriptor descriptor, TRoot root)
    {
        Descriptor = descriptor;
        Root = root;
    }
}