using System;

namespace CodeKata.RobotWars
{
    public struct Point
    {
        internal static readonly Point Zero = new Point(0, 0);

        public Int32 X;
        public Int32 Y;

        internal Point(Int32 x, Int32 y)
        {
            X = x;
            Y = y;
        }
    }
}