namespace eCommerce.Common.Tests;

public class ErrorTests
{
    private const string ErrorCodeString = "ERR-001";
    private const string ErrorMessage = "test";
    
    [Fact]
    public void Error_Should_Be_Created_With_Correct_Code_And_Message()
    {
        // Arrange
        var error = new Error(ErrorCodeString, ErrorMessage);
        
        // Assert
        error
            .Code
            .Should()
            .Be(ErrorCodeString);

        error
            .Message
            .Should()
            .Be(ErrorMessage);
    }

    [Fact]
    public void Error_Message_Prefix_Should_Be_Appended_Correctly()
    {
        // Arrange
        const string messagePrefix = "prefix";

        var error = new Error(ErrorCodeString, ErrorMessage);

        // Act
        error
            .AppendMessagePrefix(messagePrefix);

        // Assert
        error
            .Message
            .Should()
            .Be($"{messagePrefix} {ErrorMessage}");
    }

    [Fact]
    public void Additional_Message_Should_Be_Appended_Correctly()
    {
        // Arrange
        const string additionalMessage = "add";

        var error = new Error(ErrorCodeString, ErrorMessage);
        
        // Act
        error
            .AppendMessage(additionalMessage);

        // Assert
        error
            .Message
            .Should()
            .Be($"{ErrorMessage} {additionalMessage}");
    }

    [Fact]
    public void InnerException_Should_Be_Set_Correctly()
    {
        // Arrange
        var error = new Error(ErrorCodeString, ErrorMessage);
        var innerException = new Exception(ErrorMessage);

        // Act
        error
            .WithInnerException(innerException);

        // Assert
        error
            .InnerException
            .Should()
            .Be(innerException);
    }

    [Fact]
    public void ToException_And_Implicit_Cast_Should_Returns_A_Correct_ErrorException()
    {
        // Arrange
        const string additionalMessage = "add";
        const string messagePrefix = "prefix";

        var error = new Error(ErrorCodeString, ErrorMessage);
        var innerException = new Exception(ErrorMessage);
        
        // Act
        error
            .AppendMessage(additionalMessage)
            .AppendMessagePrefix(messagePrefix)
            .WithInnerException(innerException);
        
        var toExceptionResult = error.ToException();
        ErrorException implicitCastResult = error;

        // Assert
        new[]
        {
            toExceptionResult,
            implicitCastResult
        }
        .Should()
        .AllSatisfy((exception) =>
        {
            exception
                .ErrorMessage
                .Should()
                .Be($"{messagePrefix} {ErrorMessage} {additionalMessage}");

            exception
                .ErrorCode
                .Should()
                .Be(ErrorCodeString);

            exception
                .InnerException
                .Should()
                .Be(innerException);
        });
    }
}