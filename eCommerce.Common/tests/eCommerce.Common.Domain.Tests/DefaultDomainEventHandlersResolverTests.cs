using eCommerce.Common.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Common.Domain.Tests;

public class DefaultDomainEventHandlersResolverTests
{
    #region Тестовые типы

    private sealed record TestDomainEvent : DomainEvent<Guid>;

    private sealed class FirstDomainEventHandler
        : IDomainEventHandler<TestDomainEvent, Guid>
    {
        public Task Handle(TestDomainEvent domainEvent) => Task.CompletedTask;
    }

    private sealed class SecondDomainEventHandler
        : IDomainEventHandler<TestDomainEvent, Guid>
    {
        public Task Handle(TestDomainEvent domainEvent) => Task.CompletedTask;
    }

    #endregion

    [Fact]
    public void Should_Resolve_Handlers_For_A_Domain_Event()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<
            IDomainEventHandler<TestDomainEvent, Guid>,
            FirstDomainEventHandler
        >();

        serviceCollection.AddScoped<
            IDomainEventHandler<TestDomainEvent, Guid>,
            SecondDomainEventHandler
        >();

        var provider = serviceCollection.BuildServiceProvider();

        var handlersResolver =
            new DefaultDomainEventHandlersResolver<Guid>(provider);

        // Act
        var handlers = handlersResolver
            .ResolveFor<TestDomainEvent>()
            .ToArray();

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
}