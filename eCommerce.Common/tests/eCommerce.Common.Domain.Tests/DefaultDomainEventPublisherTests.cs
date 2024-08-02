using eCommerce.Common.Domain.Errors;
using Moq;

namespace eCommerce.Common.Domain.Tests;

public class DefaultDomainEventPublisherTests
{
    #region Тестовые типы

    public sealed class TestAggregateRoot(Guid identifier)
        : AggregateRoot<Guid>(identifier);

    private sealed record TestDomainEvent : DomainEvent<Guid>;

    private sealed class TestDomainEventHandler
        : IDomainEventHandler<TestDomainEvent, Guid>
    {
        public bool IsHandled { get; private set; } = false;

        public Task Handle(TestDomainEvent domainEvent)
        {
            IsHandled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class TestDomainEventApplier
        : IDomainEventApplier<TestAggregateRoot, Guid, TestDomainEvent>
    {
        public bool IsApplied { get; private set; } = false;

        public void Apply(
            TestAggregateRoot aggregateRoot,
            TestDomainEvent domainEvent
        )
        {
            IsApplied = true;
        }
    }

    #endregion

    [Fact]
    public async Task Should_Publish_A_Domain_Event()
    {
        // Arrange
        var handler = new TestDomainEventHandler();
        var applier = new TestDomainEventApplier();

        var publisher = ArrangePublisher(handler, applier);
        var aggregate = ArrangeAggregate();

        var domainEvent = new TestDomainEvent
        {
            Id = Guid.NewGuid(),
            DateStamp = DateTimeOffset.Now,
            AggregateRootId = aggregate.Root.Id
        };

        // Act
        await publisher.Publish(aggregate, domainEvent);

        // Assert
        aggregate
            .UncommittedEvents
            .Should()
            .HaveCount(1)
            .And
            .Contain(domainEvent);

        handler
            .IsHandled
            .Should()
            .BeTrue();

        applier
            .IsApplied
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Should_Throw_An_Error_On_Publishing_An_Event_From_Another_Instance()
    {
        // Arrange
        var handler = new TestDomainEventHandler();
        var applier = new TestDomainEventApplier();

        var publisher = ArrangePublisher(handler, applier);
        var aggregate = ArrangeAggregate();

        var domainEvent = new TestDomainEvent
        {
            Id = Guid.NewGuid(),
            DateStamp = DateTimeOffset.Now,
            AggregateRootId = Guid.NewGuid()
        };

        // Act
        var action = () => publisher.Publish(aggregate, domainEvent);

        // Assert
        var error = DefaultDomainEventPublisherErrors
            .DomainEventFromAnotherAggregateInstance;

        var exception = (await action
            .Should()
            .ThrowAsync<ErrorException>())
            .Which;

        exception
            .ErrorCode
            .Should()
            .Be(error.Code);

        exception
            .ErrorMessage
            .Should()
            .Contain(error.Message);
    }

    private static DefaultDomainEventPublisher<TestAggregateRoot, Guid> ArrangePublisher(
        IDomainEventHandler<TestDomainEvent, Guid> handler,
        IDomainEventApplier<TestAggregateRoot, Guid, TestDomainEvent> applier
    )
    {
        var handlersResolverMock = Mock.Of<IDomainEventHandlersResolver<Guid>>((resolver) =>
            resolver.ResolveFor<TestDomainEvent>() == new[] { handler }
        );

        var applierResolverMock = Mock.Of<IDomainEventApplierResolver<TestAggregateRoot, Guid>>((resolver) =>
            resolver.ResolveFor<TestDomainEvent>() == applier
        );

        var publisher = new DefaultDomainEventPublisher<TestAggregateRoot, Guid>(
            handlersResolverMock,
            applierResolverMock
        );

        return publisher;
    }

    private static Aggregate<TestAggregateRoot, Guid> ArrangeAggregate()
    {
        var aggregateRoot = new TestAggregateRoot(Guid.NewGuid());
        var aggregate = new Aggregate<TestAggregateRoot, Guid>(default, aggregateRoot);

        return aggregate;
    }
}