namespace eCommerce.Common.Domain.Tests;

public class AggregateDescriptorTests
{
    [Fact]
    public void AggregateDescriptor_Should_Be_Created_Correctly()
    {
        // Arrange
        var descriptor = new AggregateDescriptor
        {
            Name = "test",
            Version = new SemanticVersion(major: 1)
        };

        // Assert
        descriptor
            .Name
            .Should()
            .Be("test");

        descriptor
            .Version
            .Should()
            .Be(new SemanticVersion(major: 1));
    }

    [Fact]
    public void AggregateDescriptors_Should_Be_Equal()
    {
        // Arrange
        var firstDescriptor = new AggregateDescriptor
        {
            Name = "test",
            Version = new SemanticVersion(major: 1)
        };

        var secondDescriptor = new AggregateDescriptor
        {
            Name = "test",
            Version = new SemanticVersion(major: 1)
        };

        // Act
        var equalsResult = firstDescriptor.Equals(secondDescriptor);

        // Assert
        equalsResult.Should().BeTrue();
    }

    [Fact]
    public void AggregateDescriptors_Should_Not_Be_Equal()
    {
        // Arrange
        var firstDescriptor = new AggregateDescriptor
        {
            Name = "first",
            Version = new SemanticVersion(major: 1)
        };

        var secondDescriptor = new AggregateDescriptor
        {
            Name = "second",
            Version = new SemanticVersion(major: 1)
        };

        var thirdDescriptor = new AggregateDescriptor
        {
            Name = "first",
            Version = new SemanticVersion(major: 2)
        };

        // Act
        var firstWithSecondEqualsResult = firstDescriptor.Equals(secondDescriptor);
        var firstWithThirdEqualsResult = firstDescriptor.Equals(thirdDescriptor);

        // Assert
        firstWithSecondEqualsResult.Should().BeFalse();
        firstWithThirdEqualsResult.Should().BeFalse();
    }
}