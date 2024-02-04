using System.Text.RegularExpressions;

namespace eCommerce.Common.Tests;

public class ErrorCodeTests
{
    private static readonly Regex CommonErrorCodeRegex = new(
        pattern: @"^COMMON-[A-ZА-Я-]{1,}[-]?(?>\d{1,})?$",
        options: RegexOptions.Compiled
    );

    [Fact]
    public void Create_An_Error_Code_With_Invalid_Format_Should_Throw_COMMON_ERR_001()
    {
        // Arrange
        var errorCodeString = "ddd-83";

        // Act
        var errorCodeCreation = () => new ErrorCode(errorCodeString, CommonErrorCodeRegex);

        // Assert
        errorCodeCreation
            .Should()
            .Throw<CommonException>()
            .Which
            .ErrorCode
            .Should()
            .Be("COMMON-ERRCODE-001");
    }

    [Fact]
    public void ToString_Should_Returns_An_Error_Code_String()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var errorCode = new ErrorCode(errorCodeString, CommonErrorCodeRegex);

        // Act
        var toStringResult = errorCode.ToString();

        // Assert
        toStringResult
            .Should()
            .Be(errorCodeString);
    }

    [Fact]
    public void Implicit_To_String_Conversion_Result_Should_Be_Equals_To_Error_Code_String()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var errorCode = new ErrorCode(errorCodeString, CommonErrorCodeRegex);

        // Act
        string implicitToStringResult = errorCode;

        // Assert
        implicitToStringResult
            .Should()
            .Be(errorCodeString);
    }

    [Fact]
    public void Equals_And_Equal_Operator_Results_Should_Be_True()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var firstErrorCode = new ErrorCode(errorCodeString, CommonErrorCodeRegex);
        var secondErrorCode = new ErrorCode(errorCodeString, CommonErrorCodeRegex);

        // Act
        var errorCodesEqualsResult = firstErrorCode.Equals(secondErrorCode);
        var errorStringEqualsResule = firstErrorCode.Equals(errorCodeString);
        var equalOperatorResult = firstErrorCode == secondErrorCode;

        // Arrange
        errorCodesEqualsResult
            .Should()
            .BeTrue();

        errorStringEqualsResule
            .Should()
            .BeTrue();

        equalOperatorResult
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_Result_Should_Be_False_And_Not_Equal_Operator_Result_Should_Be_True()
    {
        // Arrange
        var firstErrorCodeString = "COMMON-ERR-001";
        var secondErrorCodeString = "COMMON-ERR-002";
        var firstErrorCode = new ErrorCode(firstErrorCodeString, CommonErrorCodeRegex);
        var secondErrorCode = new ErrorCode(secondErrorCodeString, CommonErrorCodeRegex);

        // Act
        var errorCodesEqualsResult = firstErrorCode.Equals(secondErrorCode);
        var errorStringEqualsResult = firstErrorCode.Equals(secondErrorCodeString);
        var differentTypesEqualsResult = firstErrorCode.Equals(default(int));
        var notEqualOperatorResult = firstErrorCode != secondErrorCode;

        // Assert
        errorCodesEqualsResult
            .Should()
            .BeFalse();

        errorStringEqualsResult
            .Should()
            .BeFalse();

        differentTypesEqualsResult
            .Should()
            .BeFalse();

        notEqualOperatorResult
            .Should()
            .BeTrue();
    }

    [Fact]
    public void GetHashCode_Result_Should_Be_Equals_To_Error_String_GetHashCode_Result()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var errorCode = new ErrorCode(errorCodeString, CommonErrorCodeRegex);

        // Act
        var errorCodeStringHashCode = errorCodeString.GetHashCode();
        var errorCodeHashCode = errorCode.GetHashCode();

        // Assert
        errorCodeStringHashCode
            .Should()
            .Be(errorCodeHashCode);
    }
}
