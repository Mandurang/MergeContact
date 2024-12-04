using MergeContact.Interefaces;
using FluentAssertions;

namespace MergeContactTest
{
    public class FuzzyComparerUnitTest
    {
        private readonly FuzzyComparer _comparer;

        public FuzzyComparerUnitTest()
        {
            _comparer = new FuzzyComparer();
        }

        [Theory]
        [InlineData(null, "test", 50)]
        [InlineData("test", null, 50)]
        [InlineData("", "test", 50)]
        [InlineData("test", "", 50)]
        public void AreSimilar_ShouldReturnFalse_WhenValueIsNullOrEmpty(string value1, string value2, int threshold)
        {
            // Act
            bool result = _comparer.AreSimilar(value1, value2, threshold);

            // Assert
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData("hello", "helo", 80)]
        [InlineData("HELLO", "hello", 80)]
        public void AreSimilar_ShouldReturnTrue_WhenStringsAreSimilar(string value1, string value2, int threshold)
        {
            // Act
            bool result = _comparer.AreSimilar(value1, value2, threshold);

            // Assert
            result.Should().BeTrue();
        }
    }
}