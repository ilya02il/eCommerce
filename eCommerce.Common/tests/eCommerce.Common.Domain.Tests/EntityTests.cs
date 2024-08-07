﻿namespace eCommerce.Common.Domain.Tests;

public class EntityTests
{
    private class FirstTestEntity(int identifier) : Entity<int>(identifier);

    private class SecondTestEntity(int identifier) : Entity<int>(identifier);

    [Fact]
    public void Entities_Should_Be_Equal()
    {
        // Arrange
        Entity<int> firstEntity = new FirstTestEntity(1);
        Entity<int> secondEntity = new FirstTestEntity(1);
        Entity<int>? nullEntity = null;

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
        Entity<int> firstEntity = new FirstTestEntity(1);
        Entity<int> firstEntityWithDifferentId = new FirstTestEntity(2);
        Entity<int>? nullEntity = null;
        Entity<int> secondEntity = new SecondTestEntity(1);

        // Act
        var equalsWithNotEntityResult = firstEntity
            .Equals(1);

        var equalsWithOtherTypeEntityResult = firstEntity
            .Equals(secondEntity);

        var equalsOfEntityWithDifferentIdsResult = firstEntity
            .Equals(firstEntityWithDifferentId);

        var equalOperatorOfNullResult =
            nullEntity == secondEntity;

        var equalOperatorWithNullResult =
            firstEntity == nullEntity;

        var notEqualOperatorResult =
            firstEntity != firstEntityWithDifferentId;

        // Assert
        equalsWithNotEntityResult
            .Should()
            .BeFalse();

        equalsWithOtherTypeEntityResult
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
