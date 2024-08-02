using System.Reflection;
using eCommerce.Common.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Common.Domain.Tests;

public class ServiceCollectionExtensionsTests
{
    #region Тестовые типы

    private sealed class TestAggregateRoot(Guid identifier)
        : AggregateRoot<Guid>(identifier);

    private sealed record FirstDomainEvent : DomainEvent<Guid>;

    private sealed record SecondDomainEvent : DomainEvent<Guid>;

    private sealed class FirstDomainEventHandler
        : IDomainEventHandler<FirstDomainEvent, Guid>
    {
        public Task Handle(FirstDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class SecondDomainEventHandler
        : IDomainEventHandler<FirstDomainEvent, Guid>
    {
        public Task Handle(FirstDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FirstDomainEventApplier
        : IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>
    {
        public void Apply(
            TestAggregateRoot aggregateRoot,
            FirstDomainEvent domainEvent
        )
        {
        }
    }

    private sealed class SecondDomainEventApplier
        : IDomainEventApplier<TestAggregateRoot, Guid, SecondDomainEvent>
    {
        public void Apply(
            TestAggregateRoot aggregateRoot,
            SecondDomainEvent domainEvent
        )
        {
        }
    }

    #endregion

    [Fact]
    public void AddDomainEventPublisher_Should_Register_Publisher_And_Resolvers()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDomainEventPublisher();

        var provider = services.BuildServiceProvider();

        var publisher = provider
            .GetService<IDomainEventPublisher<TestAggregateRoot, Guid>>();

        var applierResolver = provider
            .GetService<IDomainEventApplierResolver<TestAggregateRoot, Guid>>();

        var handlersResolver = provider
            .GetService<IDomainEventHandlersResolver<Guid>>();

        // Assert
        publisher
            .Should()
            .NotBeNull();

        applierResolver
            .Should()
            .NotBeNull();

        handlersResolver
            .Should()
            .NotBeNull();
    }

    [Fact]
    public void AddDomainEventPublisher_Should_Register_Publisher_And_Resolvers_As_Singletons()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDomainEventPublisher();

        var provider = services.BuildServiceProvider();

        var firstPublisher = provider
            .GetService<IDomainEventPublisher<TestAggregateRoot, Guid>>();
        var secondPublisher = provider
            .GetService<IDomainEventPublisher<TestAggregateRoot, Guid>>();

        IDomainEventPublisher<TestAggregateRoot, Guid>? thirdPublisher;

        var firstApplierResolver = provider
            .GetService<IDomainEventApplierResolver<TestAggregateRoot, Guid>>();
        var secondApplierResolver = provider
            .GetService<IDomainEventApplierResolver<TestAggregateRoot, Guid>>();

        IDomainEventApplierResolver<TestAggregateRoot, Guid>? thirdApplierResolver;

        var firstHandlersResolver = provider
            .GetService<IDomainEventHandlersResolver<Guid>>();
        var secondHandlersResolver = provider
            .GetService<IDomainEventHandlersResolver<Guid>>();

        IDomainEventHandlersResolver<Guid>? thirdHandlersResolver;

        using (var scope = provider.CreateScope())
        {
            thirdPublisher = scope
                .ServiceProvider
                .GetService<IDomainEventPublisher<TestAggregateRoot, Guid>>();

            thirdApplierResolver = scope
                .ServiceProvider
                .GetService<IDomainEventApplierResolver<TestAggregateRoot, Guid>>();

            thirdHandlersResolver = scope
                .ServiceProvider
                .GetService<IDomainEventHandlersResolver<Guid>>();
                
        }

        // Assert
        firstPublisher
            .Should()
            .Be(secondPublisher);

        firstPublisher
            .Should()
            .Be(thirdPublisher);

        firstApplierResolver
            .Should()
            .Be(secondApplierResolver);

        firstApplierResolver
            .Should()
            .Be(thirdApplierResolver);

        firstHandlersResolver
            .Should()
            .Be(secondHandlersResolver);

        firstHandlersResolver
            .Should()
            .Be(thirdHandlersResolver);
    }

    [Fact]
    public void AddDomainEventHandlers_Should_Register_All_Handlers_From_Assembly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDomainEventHandlers(Assembly.GetExecutingAssembly());

        var provider = services.BuildServiceProvider();
        var handlers = provider
            .GetServices<IDomainEventHandler<FirstDomainEvent, Guid>>()
            .ToList();

        // Assert
        handlers
            .Should()
            .HaveCount(2);

        handlers
            .First()
            .Should()
            .Match((handler) => handler is FirstDomainEventHandler);

        handlers
            .Last()
            .Should()
            .Match((handler) => handler is SecondDomainEventHandler);
    }

    [Fact]
    public void AddDomainEventHandlers_Should_Register_Handlers_As_Scoped()
    {
        // Arrange
        var services = new ServiceCollection();

        List<IDomainEventHandler<FirstDomainEvent, Guid>> firstScopeHandlersFirstList;
        List<IDomainEventHandler<FirstDomainEvent, Guid>> firstScopeHandlersSecondList;
        List<IDomainEventHandler<FirstDomainEvent, Guid>> secondScopeHandlersFirstList;
        List<IDomainEventHandler<FirstDomainEvent, Guid>> secondScopeHandlersSecondList;

        // Act
        services.AddDomainEventHandlers(Assembly.GetExecutingAssembly());

        var provider = services.BuildServiceProvider();

        using (var firstScope = provider.CreateScope())
        {
            firstScopeHandlersFirstList = firstScope
                .ServiceProvider
                .GetServices<IDomainEventHandler<FirstDomainEvent, Guid>>()
                .ToList();

            firstScopeHandlersSecondList = firstScope
                .ServiceProvider
                .GetServices<IDomainEventHandler<FirstDomainEvent, Guid>>()
                .ToList();
        }

        using (var secondScope = provider.CreateScope())
        {
            secondScopeHandlersFirstList = secondScope
                .ServiceProvider
                .GetServices<IDomainEventHandler<FirstDomainEvent, Guid>>()
                .ToList();

            secondScopeHandlersSecondList = secondScope
                .ServiceProvider
                .GetServices<IDomainEventHandler<FirstDomainEvent, Guid>>()
                .ToList();
        }

        // Assert
        firstScopeHandlersFirstList
            .Should()
            .NotIntersectWith(secondScopeHandlersFirstList);

        firstScopeHandlersFirstList
            .Should()
            .BeEquivalentTo(firstScopeHandlersSecondList);

        secondScopeHandlersFirstList
            .Should()
            .BeEquivalentTo(secondScopeHandlersSecondList);
    }

    [Fact]
    public void AddDomainEventAppliers_Should_Register_All_Appliers_From_Assembly()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddDomainEventAppliers(Assembly.GetExecutingAssembly());

        var provider = services.BuildServiceProvider();

        var firstApplier = provider
            .GetService<IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>>();

        var secondApplier = provider
            .GetService<IDomainEventApplier<TestAggregateRoot, Guid, SecondDomainEvent>>();

        // Assert
        firstApplier
            .Should()
            .Match((applier) => applier is FirstDomainEventApplier);

        secondApplier
            .Should()
            .Match((applier) => applier is SecondDomainEventApplier);
    }

    [Fact]
    public void AddDomainEventHandlers_Should_Register_Appliers_As_Scoped()
    {
        // Arrange
        var services = new ServiceCollection();

        IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent> firstScopeFirstApplier;
        IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent> firstScopeSecondApplier;
        IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent> secondScopeFirstApplier;
        IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent> secondScopeSecondApplier;

        // Act
        services.AddDomainEventAppliers(Assembly.GetExecutingAssembly());

        var provider = services.BuildServiceProvider();

        using (var firstScope = provider.CreateScope())
        {
            firstScopeFirstApplier = firstScope
                .ServiceProvider
                .GetRequiredService<IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>>();

            firstScopeSecondApplier = firstScope
                .ServiceProvider
                .GetRequiredService<IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>>();
        }

        using (var secondScope = provider.CreateScope())
        {
            secondScopeFirstApplier = secondScope
                .ServiceProvider
                .GetRequiredService<IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>>();

            secondScopeSecondApplier = secondScope
                .ServiceProvider
                .GetRequiredService<IDomainEventApplier<TestAggregateRoot, Guid, FirstDomainEvent>>();
        }

        // Assert
        firstScopeFirstApplier
            .Should()
            .NotBe(secondScopeFirstApplier);

        firstScopeFirstApplier
            .Should()
            .Be(firstScopeSecondApplier);

        secondScopeFirstApplier
            .Should()
            .Be(secondScopeSecondApplier);
    }
}