using eCommerce.Common.Domain.Errors;

namespace eCommerce.Common.Domain.Tests;

public class AggregateRootTests
{
    private class TestAggregateRoot(int identifier)
        : AggregateRoot<int>(identifier)
    {
        public override string AggregateName => nameof(AggregateRootTests);
    
        public override Version AggregateVersion => new(major: 1);
    
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
            PublishDomainEvent(
                new TestDomainEvent(
                    Identifier,
                    propertyValue
                ),
                TimeProvider.System
            );
        }

        public void SomeActionWithEventFromAnotherAggregate()
        {
            PublishDomainEvent(
                new TestEntityFromAnotherAggregate(
                    Identifier
                ),
                TimeProvider.System
            );
        }

        public void SomeActionWithEventWithNotCompatibleVersion()
        {
            PublishDomainEvent(
                new TestEntityWithNotCompatibleVersion(
                    Identifier
                ),
                TimeProvider.System
            );
        }
    
        protected override void ApplyDomainEventInternal(
            DomainEvent<int> domainEvent,
            TimeProvider timeProvider
        )
        {
            if (domainEvent is TestDomainEvent testDomainEvent)
            {
                SomeProperty = testDomainEvent.SomePropertyValue;
            }
        }
    }
    
    private record TestDomainEvent : DomainEvent<int>
    {
        public override string AggregateName => nameof(AggregateRootTests);
    
        public override Version AggregateVersion => new(major: 1);
    
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

    private record TestEntityFromAnotherAggregate : DomainEvent<int>
    {
        public override string AggregateName => "test";

        public override Version AggregateVersion => new(major: 1);

        public TestEntityFromAnotherAggregate(int aggregateRootId)
            : base(aggregateRootId)
        {
        }
    }
    
    private record TestEntityWithNotCompatibleVersion : DomainEvent<int>
    {
        public override string AggregateName => nameof(AggregateRootTests);

        public override Version AggregateVersion => new(major: 2);

        public TestEntityWithNotCompatibleVersion(int aggregateRootId) 
            : base(aggregateRootId)
        {
        }
    }

    [Fact]
    public void Publish_And_Apply_A_DomainEvent()
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
    public void Throw_An_Error_On_Publishing_Event_From_Another_Aggregate()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(
            identifier: 1,
            propertyValue: 1
        );

        // Act
        var action = () => aggregate
            .SomeActionWithEventFromAnotherAggregate();

        // Assert
        var error = AggregateRootErrors
            .DomainEventHasDifferentAggregate;
        
        var exception = action
            .Should()
            .ThrowExactly<ErrorException>()
            .Which;

        exception
            .ErrorCode
            .Should()
            .Be(error.Code);

        exception
            .ErrorMessage
            .Should()
            .Be(error.Message);
    }

    [Fact]
    public void Throw_An_Error_On_Applying_Event_From_Another_Aggregate_Instance()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(
            identifier: 1,
            propertyValue: 1
        );

        var eventFromAnotherAggregateInstance = new TestDomainEvent(
            aggregateRootId: 2,
            somePropertyValue: 1
        );

        // Act
        var action = () => aggregate.ApplyDomainEvent(
            eventFromAnotherAggregateInstance,
            TimeProvider.System
        );

        // Assert
        var error = AggregateRootErrors
            .DomainEventFromAnotherAggregateInstance;

        var exception = action
            .Should()
            .ThrowExactly<ErrorException>()
            .Which;

        exception
            .ErrorCode
            .Should()
            .Be(error.Code);

        exception
            .ErrorMessage
            .Should()
            .Be(error.Message);
    }

    [Fact]
    public void Throw_An_Error_On_Publishing_Event_With_Not_Compatible_Aggregate_Version()
    {
        // Arrange
        var aggregate = new TestAggregateRoot(
            identifier: 1,
            propertyValue: 1
        );

        // Act
        var action = () => aggregate
            .SomeActionWithEventWithNotCompatibleVersion();

        // Assert
        var error = AggregateRootErrors
            .DomainEventHasNotCompatibleAggregateVersion;

        var exception = action
            .Should()
            .ThrowExactly<ErrorException>()
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

    [Fact]
    public void Publish_Events_In_Correct_Order()
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
                domainEvent is TestDomainEvent { SomePropertyValue: 2 });
    
        var secondEvent = aggregate
            .DomainEvents
            .First(domainEvent =>
                domainEvent is TestDomainEvent { SomePropertyValue: 3 });
    
        aggregate
            .DomainEvents
            .Should()
            .ContainInOrder([ firstEvent, secondEvent ]);
    }
}
