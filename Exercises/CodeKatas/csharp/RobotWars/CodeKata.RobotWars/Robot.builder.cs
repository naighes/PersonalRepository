using System;

namespace CodeKata.RobotWars
{
    public partial class Robot
    {
        internal static Robot New(String input)
        {
            var array = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            if (array.Length != 3)
                throw new FormatException();

            var orientation = array[2][0];

            if (Array.IndexOf(CardinalCompassPoints, orientation) == -1)
                throw new FormatException();

            var x = Int32.Parse(array[0]);
            var y = Int32.Parse(array[1]);

            return new Robot(new Point(x, y), orientation);
        }
    }
}