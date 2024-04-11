namespace eCommerce.Common.Domain.Tests;

public class EntityTests
{
    private class FirstTestEntity(int identifier)
        : Entity<int>(identifier)
    {
        public override string AggregateName => nameof(EntityTests);

        public override Version AggregateVersion => new(major: 1);
    }

    private class SecondTestEntity(int identifier)
        : Entity<int>(identifier)
    {
        public override string AggregateName => nameof(EntityTests);

        public override Version AggregateVersion => new(major: 1);
    }

    private class ThirdTestEntity(int identifier)
        : Entity<int>(identifier)
    {
        public override string AggregateName => nameof(EntityTests);

        public override Version AggregateVersion => new(major: 2);
    }

    private class FourthTestEntity(int identifier)
        : Entity<int>(identifier)
    {
        public override string AggregateName => "test";

        public override Version AggregateVersion => new(major: 1);
    }

    [Fact]
    public void Entities_Should_Be_Equal()
    {
        // Arrange
        var firstEntity = new FirstTestEntity(1);
        var secondEntity = new FirstTestEntity(1);
        FirstTestEntity nullEntity = null;

        // Act
        var equalsResult = firstEntity
            .Equals(secondEntity);

        var equalOperatorResult =
            firstEntity == secondEntity;

        var nullEqualOperatorResult =
            nullEntity == null;

        // Assert
        equalsResult
            .Should()
            .BeTrue();

        equalOperatorResult
            .Should()
            .BeTrue();

        nullEqualOperatorResult
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Entities_Should_Not_Be_Equal()
    {
        // Arrange
        var firstEntity = new FirstTestEntity(1);
        var firstEntityWithDifferentId = new FirstTestEntity(2);
        FirstTestEntity? nullFirstEntity = null;

        var secondEntity = new SecondTestEntity(1);
        var thirdEntity = new ThirdTestEntity(1);
        var fourthEntity = new FourthTestEntity(1);

        // Act
        var equalsWithNotEntityResult = firstEntity
            .Equals(1);

        var equalsWithOtherTypeEntityResult = firstEntity
            .Equals(secondEntity);

        var equalsWithAnotherAggregateVersionResult = firstEntity
            .Equals(thirdEntity);

        var equalsWithAnotherAggregateEntityResult = firstEntity
            .Equals(fourthEntity);

        var equalsOfEntityWithDifferentIdsResult = firstEntity
            .Equals(firstEntityWithDifferentId);

        var equalOperatorOfNullResult =
            nullFirstEntity == secondEntity;

        var equalOperatorWithNullResult =
            firstEntity == nullFirstEntity;

        var notEqualOperatorResult =
            firstEntity != firstEntityWithDifferentId;

        // Assert
        equalsWithNotEntityResult
            .Should()
            .BeFalse();

        equalsWithOtherTypeEntityResult
            .Should()
            .BeFalse();

        equalsWithAnotherAggregateVersionResult
            .Should()
            .BeFalse();

        equalsWithAnotherAggregateEntityResult
            .Should()
            .BeFalse();

        equalsOfEntityWithDifferentIdsResult
            .Should()
            .BeFalse();

        equalOperatorOfNullResult
            .Should()
            .BeFalse();

        equalOperatorWithNullResult
            .Should()
            .BeFalse();

        notEqualOperatorResult
            .Should()
            .BeTrue();
    }

    [Fact]
    public void HashCodes_Should_Be_Equal()
    {
        // Arrange
        var firstEntity = new FirstTestEntity(1);
        var secondEntity = new FirstTestEntity(1);

        // Act
        var firstHashCode = firstEntity.GetHashCode();
        var secondHashCode = secondEntity.GetHashCode();

        // Assert
        firstHashCode
            .Should()
            .Be(secondHashCode);
    }

    [Fact]
    public void HashCodes_Should_Not_Be_Equal()
    {
        // Arrange
        var firstEntity = new FirstTestEntity(1);
        var secondEntity = new SecondTestEntity(1);

        // Act
        var firstHashCode = firstEntity.GetHashCode();
        var secondHashCode = secondEntity.GetHashCode();

        // Assert
        firstHashCode
            .Should()
            .NotBe(secondHashCode);
    }
}
