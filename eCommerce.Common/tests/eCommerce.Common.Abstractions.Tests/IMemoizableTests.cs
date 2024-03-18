namespace eCommerce.Common.Abstractions.Tests;

public class MemoizableTests
{
    private record TestMemento
    {
        public int FirstProperty { get; init; }

        public string SecondProperty { get; init; } = null!;
    }
    
    private record TestMemoizable : IMemoizable<TestMemoizable, TestMemento>
    {
        public int FirstProperty { get; private init; }

        public string SecondProperty { get; private init; } = null!;

        public TestMemoizable() { }

        public TestMemoizable(int firstProperty, string secondProperty)
        {
            FirstProperty = firstProperty;
            SecondProperty = secondProperty;
        }
        
        public static TestMemoizable Restore(TestMemento memento)
        {
            return new TestMemoizable()
            {
                FirstProperty = memento.FirstProperty,
                SecondProperty = memento.SecondProperty
            };
        }

        public TestMemento Memoize()
        {
            return new TestMemento()
            {
                FirstProperty = FirstProperty,
                SecondProperty = SecondProperty
            };
        }
    }

    [Fact]
    public void Memoizable_Should_Memoize_And_Restore_Right()
    {
        // Arrange
        var memoizable = new TestMemoizable(
            firstProperty: 1,
            secondProperty: "second"
        );
        
        // Act
        var memento = memoizable.Memoize();
        var restoredMemoizable = TestMemoizable.Restore(memento);

        // Assert
        restoredMemoizable
            .Should()
            .Be(memoizable);
    }
}
