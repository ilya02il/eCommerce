namespace eCommerce.Common.Tests;

public class VersionTests
{
    [Fact]
    public void Versions_Should_Create_Correctly()
    {
        // Arrange
        var fullVersion = new Version(
            major: 1, 
            minor: 2,
            patch: 3,
            build: 4,
            postfix: "five"
        );

        var versionWithoutPostfix = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4
        );

        var versionWithoutPostfixAndBuild = new Version(
            major: 1,
            minor: 2,
            patch: 3
        );

        var versionWithOnlyMajorAndMinor = new Version(
            major: 1,
            minor: 2
        );

        var versionWithOnlyMajor = new Version(major: 1);

        // Assert
        new[]
        {
            fullVersion,
            versionWithoutPostfix,
            versionWithoutPostfixAndBuild,
            versionWithOnlyMajorAndMinor,
            versionWithOnlyMajor
        }
        .Should()
        .AllSatisfy((version) => version.Major.Should().Be(1));
        
        new[]
        {
            fullVersion,
            versionWithoutPostfix,
            versionWithoutPostfixAndBuild,
            versionWithOnlyMajorAndMinor
        }
        .Should()
        .AllSatisfy((version) => version.Minor.Should().Be(2));
        
        new[]
        {
            fullVersion,
            versionWithoutPostfix,
            versionWithoutPostfixAndBuild
        }
        .Should()
        .AllSatisfy((version) => version.Patch.Should().Be(3));
        
        new[]
        {
            fullVersion,
            versionWithoutPostfix
        }
        .Should()
        .AllSatisfy((version) => version.Build.Should().Be(4));

        fullVersion
            .Postfix
            .Should()
            .Be("five");
    }

    [Fact]
    public void Versions_Should_Be_Equal()
    {
        // Arrange
        var firstVersion = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4,
            postfix: "five"
        );

        var secondVersion = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4,
            postfix: "five"
        );

        // Act
        var equalsResult = firstVersion.Equals(secondVersion);
        var equalityOperatorResult = firstVersion == secondVersion;

        // Assert
        equalsResult.Should().BeTrue();
        equalityOperatorResult.Should().BeTrue();
    }

    [Fact]
    public void Versions_Should_Not_Be_Equal()
    {
        // Arrange
        var firstVersion = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4,
            postfix: "five"
        );

        var secondVersion = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4
        );

        // Act
        var equalsResult = firstVersion.Equals(secondVersion);
        var equalityOperatorResult = firstVersion == secondVersion;

        // Assert
        equalsResult.Should().BeFalse();
        equalityOperatorResult.Should().BeFalse();
    }

    [Fact]
    public void Version_ToString_Should_Return_Correct_String()
    {
        // Arrange
        var versionWithoutPostfix = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4
        );
        
        var versionWithPostfix = new Version(
            major: 1,
            minor: 2,
            patch: 3,
            build: 4,
            postfix: "five"
        );

        // Act
        var stringifiedVersionWithoutPostfix =
            versionWithoutPostfix.ToString();

        var stringifiedVersionWithPostfix =
            versionWithPostfix.ToString();

        // Assert
        stringifiedVersionWithoutPostfix
            .Should()
            .Be("1.2.3.4");

        stringifiedVersionWithPostfix
            .Should()
            .Be("1.2.3.4-five");
    }
}