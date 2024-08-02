using eCommerce.Common.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Common.Domain.Tests;

public class DefaultDomainEventApplierResolverTests
{
    #region Тестовые типы

    private sealed class TestAggregateRoot(Guid identifier)
        : AggregateRoot<Guid>(identifier);

    private sealed record TestDomainEvent : DomainEvent<Guid>;

    private sealed class TestDomainEventApplier
        : IDomainEventApplier<TestAggregateRoot, Guid, TestDomainEvent>
    {
        public void Apply(
            TestAggregateRoot aggregateRoot,
            TestDomainEvent domainEvent
        )
        {
        }
    }

    #endregion

    [Fact]
    public void Should_Resolve_An_Applier_For_A_Domain_Event()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<
            IDomainEventApplier<TestAggregateRoot, Guid, TestDomainEvent>,
            TestDomainEventApplier
        >();

        var provider = serviceCollection.BuildServiceProvider();

        var applierResolver =
            new DefaultDomainEventApplierResolver<TestAggregateRoot, Guid>(provider);

        // Act
        var domainEventApplier = applierResolver.ResolveFor<TestDomainEvent>();

        // Assert
        domainEventApplier
            .Should()
            .Match((applier) => applier is TestDomainEventApplier);
    }
}