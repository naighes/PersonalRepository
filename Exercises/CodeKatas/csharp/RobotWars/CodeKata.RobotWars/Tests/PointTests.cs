using Xunit;

namespace CodeKata.RobotWars.Tests
{
    public class PointTests
    {
        [Fact]
        public void PointEquality()
        {
            var p1 = new Point(1, 1);
            Assert.Equal(p1, p1);

            var p2 = new Point(1, 1);
            Assert.Equal(p1, p2);

            var p3 = new Point(1, 2);
            Assert.NotEqual(p1, p3);
        }
    }
}