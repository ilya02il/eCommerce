namespace eCommerce.Common.Tests;

public class BaseExceptionTests
{
    const string ErrorCodeString = "COMMON-ERR-001";
    const string ErrorMessage = "test";
    
    private class TestException(
        string errorCode,
        string errorMessage,
        Exception innerException
    ) : BaseException(
        errorCode,
        errorMessage,
        innerException
    );

    [Fact]
    public void BaseException_Should_Have_Right_ErrorCode_And_ErrorMessage()
    {
        // Arrange
        var exception = new CommonException(ErrorCodeString, ErrorMessage);

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
    public void BaseException_Should_Have_Right_Message()
    {
        // Arrange
        var exception = new CommonException(ErrorCodeString, ErrorMessage);

        // Assert
        exception
            .Message
            .Should()
            .Be($"{ErrorCodeString}: {ErrorMessage}");
    }

    [Fact]
    public void BaseException_Should_Have_Referenced_Exception_Message_And_Right_ErrorCode()
    {
        // Arrange
        var referencedException = new Exception(ErrorMessage);
        var exception = new CommonException(ErrorCodeString, referencedException);

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
    public void BaseException_Should_Have_Inner_Exception_And_Right_ErrorCode_And_Error_Message()
    {
        // Arrange
        const string errorCodeString = "ERR-001";
        var innerException = new Exception(ErrorMessage);

        var exception = new TestException(
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
