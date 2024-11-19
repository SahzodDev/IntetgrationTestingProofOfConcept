using FluentAssertions;

namespace FluentAssertionsDemo
{
    public class UnitTest1
    {
        [Fact]
        public void TestToCheckOneVariableLessThanOther()
        {
            var x = 5;
            var y = 10;
            x.Should().BeLessThan(y);
        }
    }
}