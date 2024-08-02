namespace eCommerce.Common.Tests;

public class ErrorExceptionTests
{
    const string ErrorCodeString = "COMMON-ERR-001";
    const string ErrorMessage = "test";

    [Fact]
    public void Should_Have_Right_ErrorCode_And_ErrorMessage()
    {
        // Arrange
        var exception = new ErrorException(ErrorCodeString, ErrorMessage, null);

        // Assert
        exception
            .ErrorCode
            .Should()
            .Be(ErrorCodeString);

        exception
            .ErrorMessage
            .Should()
            .Be(ErrorMessage);
    }

    [Fact]
    public void Should_Have_Right_Message()
    {
        // Arrange
        var exception = new ErrorException(ErrorCodeString, ErrorMessage, null);

        // Assert
        exception
            .Message
            .Should()
            .Be($"{ErrorCodeString}: {ErrorMessage}");
    }

    [Fact]
    public void Should_Have_Right_InnerException()
    {
        // Arrange
        var innerException = new Exception(ErrorMessage);
        var exception = new ErrorException(
            ErrorCodeString,
            ErrorMessage,
            innerException
        );

        // Assert
        exception
            .InnerException
            .Should()
            .Be(innerException);
    }

    [Fact]
    public void Should_Have_Inner_Exception_And_Right_ErrorCode_Error_Message_And_InnerException()
    {
        // Arrange
        const string errorCodeString = "ERR-001";
        var innerException = new Exception(ErrorMessage);

        var exception = new ErrorException(
            errorCodeString,
            ErrorMessage,
            innerException
        );

        // Assert
        exception
            .InnerException
            .Should()
            .Be(innerException);

        exception
            .ErrorCode
            .Should()
            .Be(errorCodeString);

        exception
            .ErrorMessage
            .Should()
            .Be(ErrorMessage);

        exception
            .Message
            .Should()
            .Be($"{errorCodeString}: {ErrorMessage}");
    }
}
