namespace eCommerce.Common.Tests;

public class EventTests
{
    private record TestEvent : Event
    {
        public TestEvent() : base() { }

        public TestEvent(TimeProvider timeProvider) : base(timeProvider) { }
    }

    private class TestTimeProvider : TimeProvider
    {
        private static readonly DateTimeOffset TestUtcNow =
            DateTimeOffset.UtcNow;

        public override DateTimeOffset GetUtcNow() => TestUtcNow;
    }

    private readonly TimeProvider _timeProvider;

    public EventTests()
    {
        _timeProvider = new TestTimeProvider();
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
        var testEvent = new TestEvent(_timeProvider);

        // Assert
        testEvent
            .DateStamp
            .Should()
            .Be(_timeProvider.GetUtcNow());
    }
}
