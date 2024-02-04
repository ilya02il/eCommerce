namespace eCommerce.Common.Domain;

/// <summary>
/// Базовый класс сущности.
/// </summary>
/// <typeparam name="TId">
/// Тип идентификатора сущности.
/// </typeparam>
public abstract class Entity<TId>
{
    /// <summary>
    /// Идентификатор сущности.
    /// Это свойство идентифицирует сущность.
    /// Сущности, имеющие одинаковые идентификаторы
    /// считаются равными, даже если другие их поля
    /// имеют различные значения.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Создать новый экземпляр сущности с
    /// идентификатором равным <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">
    /// Идентификатор сущности.
    /// </param>
    protected Entity(TId identifier)
    {
        Id = identifier;
    }

    /// <summary>
    /// Определить равен ли указанный объект
    /// (<paramref name="obj"/>) данной сущности.
    /// </summary>
    /// <param name="obj">
    /// Объект, сравниваемый с данной сущностью.
    /// </param>
    /// <returns>
    /// <see langword="true"/> - если выполняются следующие условия:<br/>
    /// <list type="bullet">
    ///     <item>
    ///         <description>Объект является сущностью с типом как у данной сущности.</description>
    ///     </item>
    ///     <item>
    ///         <description>Идентификаторы сущностей равны.</description>
    ///     </item>
    /// </list>
    /// <see langword="false"/> - во всех остальных случаях.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        return Id!.Equals(other.Id);
    }

    /// <summary>
    /// Определить равны ли сущности.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> -
    /// если <paramref name="left"/> и <paramref name="right"/>
    /// сущности равны <see langword="null"/>.
    /// <br/>
    /// <see langword="false"/> -
    /// если <paramref name="left"/> или <paramref name="right"/>
    /// сущность равна <see langword="null"/>.
    /// <br/>
    /// Если предыдущие условия не выполняются,
    /// то возвращается результат работы метода <see cref="Equals(object?)"/>.
    /// </returns>
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Определить не равны ли сущности.
    /// </summary>
    /// <returns>
    /// Значение, противоположное результату
    /// <see cref="operator ==(Entity{TId}, Entity{TId})"/>.
    /// </returns>
    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return (left == right) is false;
    }

    /// <inheritdoc cref="object.GetHashCode()"/>
    public override int GetHashCode()
    {
        return GetType().Name.GetHashCode() ^ Id!.GetHashCode();
    }
}