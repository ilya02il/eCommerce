namespace eCommerce.Common.Domain.Tests;

public class AggregateRootTests
{
    private class TestAggregateRoot(int identifier)
        : AggregateRoot<int>(identifier)
    {
        public int Identifier { get; private init; }

        public int SomeProperty { get; private set; }

        public TestAggregateRoot(int identifier, int propertyValue)
            : this(identifier)
        {
            Identifier = identifier;
            SomeProperty = propertyValue;
        }

        public void SomeAction(int propertyValue)
        {
            PublishDomainEvent(new TestDomainEvent(
                Identifier,
                propertyValue
            ));
        }

        public override void ApplyDomainEvent(DomainEvent<int> domainEvent)
        {
            if (domainEvent is TestDomainEvent testDomainEvent)
            {
                SomeProperty = testDomainEvent.SomePropertyValue;
            }
        }
    }

    private record TestDomainEvent : DomainEvent<int>
    {
        public int SomePropertyValue { get; private init; }

        public TestDomainEvent(
            int aggregateRootId,
            int somePropertyValue
        )
        : base(aggregateRootId)
        {
            SomePropertyValue = somePropertyValue;
        }
    }

    [Fact]
    public void Aggregate_Should_Publish_And_Apply_A_DomainEvent()
    {
        const int modifiedPropertyValue = 2;

        // Arrange
        var aggregate = new TestAggregateRoot(1, 1);

        // Act
        aggregate.SomeAction(modifiedPropertyValue);

        // Assert
        aggregate
            .DomainEvents
            .Should()
            .Contain(domainEvent =>
                ((TestDomainEvent)domainEvent).AggregateRootId == aggregate.Identifier &&
                ((TestDomainEvent)domainEvent).SomePropertyValue == modifiedPropertyValue
            );

        aggregate
            .SomeProperty
            .Should()
            .Be(modifiedPropertyValue);
    }

    [Fact]
    public void Aggregate_Should_Publish_Events_In_Correct_Order()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(1, 1);

        // Act
        aggregate.SomeAction(2);
        aggregate.SomeAction(3);

        // Assert
        var firstEvent = aggregate
            .DomainEvents
            .First(domainEvent =>
                domainEvent is TestDomainEvent testDomainEvent &&
                testDomainEvent.SomePropertyValue == 2
            );

        var secondEvent = aggregate
            .DomainEvents
            .First(domainEvent =>
                domainEvent is TestDomainEvent testDomainEvent &&
                testDomainEvent.SomePropertyValue == 3
            );

        aggregate
            .DomainEvents
            .Should()
            .ContainInOrder([ firstEvent, secondEvent ]);
    }
}
