namespace eCommerce.Common.Tests;

public class BaseExceptionTests
{
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
        var errorCodeString = "COMMON-ERR-001";
        var errorMessage = "test";
        var exception = new CommonException(errorCodeString, errorMessage);

        // Assert
        exception
            .ErrorCode
            .Should()
            .Be(errorCodeString);

        exception
            .ErrorMessage
            .Should()
            .Be(errorMessage);
    }

    [Fact]
    public void BaseException_Should_Have_Right_Message()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var errorMessage = "test";
        var exception = new CommonException(errorCodeString, errorMessage);

        // Assert
        exception
            .Message
            .Should()
            .Be($"{errorCodeString}: {errorMessage}");
    }

    [Fact]
    public void BaseException_Should_Have_Refernced_Exception_Message_And_Right_ErrorCode()
    {
        // Arrange
        var errorCodeString = "COMMON-ERR-001";
        var referencedExceptionMessage = "test";
        var referencedException = new Exception(referencedExceptionMessage);
        var exception = new CommonException(errorCodeString, referencedException);

        // Assert
        exception
            .ErrorCode
            .Should()
            .Be(errorCodeString);

        exception
            .ErrorMessage
            .Should()
            .Be(referencedExceptionMessage);
    }

    [Fact]
    public void BaseException_Should_Have_Inner_Exception_And_Right_ErrorCode_And_Error_Message()
    {
        // Arrange
        var errorCodeString = "ERR-001";
        var errorMessage = "test";
        var innerException = new Exception("test");
        var exception = new TestException(errorCodeString, errorMessage, innerException);

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
            .Be(errorMessage);

        exception
            .Message
            .Should()
            .Be($"{errorCodeString}: {errorMessage}");
    }
}
