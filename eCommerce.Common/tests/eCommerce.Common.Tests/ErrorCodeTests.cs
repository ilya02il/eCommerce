using eCommerce.Common.Errors;

namespace eCommerce.Common.Tests;

public class ErrorCodeTests
{
    private const string ErrorCodeString = "COMMON-ERR-001";

    [Fact]
    public void Create_An_Error_Code_With_Valid_Format_Should_Not_Throw_An_Error()
    {
        // Arrange
        string[] correctErrorCodesStrings =
        [
            "ABC-123",
            "ABC-ABC-123"
        ];

        // Act
        var errorCodesCreators = correctErrorCodesStrings
            .Select<string, Func<ErrorCode>>(
                (errorCodeString) => () => new ErrorCode(errorCodeString)
            );

        // Assert
        errorCodesCreators
            .Should()
            .AllSatisfy((createAction) =>
            {
                createAction
                    .Should()
                    .NotThrow();
            });
    }
    
    [Fact]
    public void Create_An_Error_Code_With_Invalid_Format_Should_Throw_An_Error()
    {
        // Arrange
        string[] incorrectErrorCodesStrings =
        [
            "АБВ-123",
            "abc-123",
            "aBC-123",
            "ABC",
            "-ABC-123",
            "ABC--123",
            "ABC--ABC-123"
        ];

        // Act
        var wrongErrorCodesCreators = incorrectErrorCodesStrings
            .Select<string, Func<ErrorCode>>(
                (errorCodeString) => () => new ErrorCode(errorCodeString)
            );

        // Assert
        wrongErrorCodesCreators
            .Should()
            .AllSatisfy((createAction) =>
            {
                var exception = createAction
                    .Should()
                    .Throw<ErrorException>()
                    .Which;

                var error = ErrorCodeErrors
                    .AnInputStringDoesNotMatchThePattern;

                exception
                    .ErrorCode
                    .Should()
                    .Be(error.Code);

                exception
                    .ErrorMessage
                    .Should()
                    .Contain(error.Message);
            });
    }

    [Fact]
    public void ToString_Should_Returns_An_Error_Code_String()
    {
        // Arrange
        var errorCode = new ErrorCode(ErrorCodeString);

        // Act
        var toStringResult = errorCode.ToString();

        // Assert
        toStringResult
            .Should()
            .Be(ErrorCodeString);
    }

    [Fact]
    public void Implicit_To_String_Conversion_Result_Should_Be_Equals_To_Error_Code_String()
    {
        // Arrange
        var errorCode = new ErrorCode(ErrorCodeString);

        // Act
        string implicitToStringResult = errorCode;

        // Assert
        implicitToStringResult
            .Should()
            .Be(ErrorCodeString);
    }

    [Fact]
    public void Equals_And_Equal_Operator_Results_Should_Be_True()
    {
        // Arrange
        var firstErrorCode = new ErrorCode(ErrorCodeString);
        var secondErrorCode = new ErrorCode(ErrorCodeString);

        // Act
        var errorCodesEqualsResult = firstErrorCode.Equals(secondErrorCode);
        var errorStringEqualsResult = firstErrorCode.Equals(ErrorCodeString);
        var equalOperatorResult = firstErrorCode == secondErrorCode;

        // Arrange
        errorCodesEqualsResult
            .Should()
            .BeTrue();

        errorStringEqualsResult
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
        const string anotherErrorCodeString = "COMMON-ERR-002";
        var firstErrorCode = new ErrorCode(ErrorCodeString);
        var secondErrorCode = new ErrorCode(anotherErrorCodeString);

        // Act
        var errorCodesEqualsResult = firstErrorCode.Equals(secondErrorCode);
        var errorStringEqualsResult = firstErrorCode.Equals(anotherErrorCodeString);
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
        var errorCode = new ErrorCode(ErrorCodeString);

        // Act
        var errorCodeStringHashCode = ErrorCodeString.GetHashCode();
        var errorCodeHashCode = errorCode.GetHashCode();

        // Assert
        errorCodeStringHashCode
            .Should()
            .Be(errorCodeHashCode);
    }
}
