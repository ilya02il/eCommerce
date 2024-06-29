using Moq;

namespace eCommerce.Common.Tests;

public class EventTests
{
    private record TestEvent : Event
    {
        public TestEvent(TimeProvider? timeProvider = null) : base(timeProvider) { }
    }

    [Fact]
    public void Event_Id_Should_Not_Equals_To_Guid_Empty_And_Guid_Default()
    {
        // Arrange
        var testEvent = new TestEvent();

        // Assert
        testEvent
            .Id
            .Should()
            .NotBe(Guid.Empty)
            .And
            .NotBe(default(Guid));
    }

    [Fact]
    public void Event_DateStamp_Should_Be_Equals_To_UtcNow()
    {
        // Arrange
        var timeProviderMock = Mock.Of<TimeProvider>((timeProvider) =>
            timeProvider.GetUtcNow() == DateTimeOffset.UtcNow
        );

        var testEvent = new TestEvent(timeProviderMock);

        // Assert
        testEvent
            .DateStamp
            .Should()
            .Be(timeProviderMock.GetUtcNow());
    }
}
