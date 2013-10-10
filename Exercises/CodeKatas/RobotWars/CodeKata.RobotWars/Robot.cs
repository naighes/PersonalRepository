using System;
using System.Collections.Generic;

namespace CodeKata.RobotWars
{
    public class Robot
    {
        private Robot(Point position, Char orientation)
        {
            _position = position;
            _orientation = Array.IndexOf(CardinalCompassPoints, orientation);
        }

        public Point Position
        {
            get { return _position; }
        }
        private Point _position;

        public Char Orientation
        {
            get
            {
                return CardinalCompassPoints[_orientation & (CardinalCompassPoints.Length - 1)];
            }
        }
        private Int32 _orientation;

        internal void Turn(Char input)
        {
            RotateCommands[input](this);
        }

        public void Move(Char input)
        {
            MovementCommands[Orientation](this);
        }

        private static readonly IDictionary<Char, Action<Robot>> MovementCommands =
            new Dictionary<Char, Action<Robot>>
                {
                    {'N', robot => robot._position.Y++},
                    {'W', robot => robot._position.X--},
                    {'S', robot => robot._position.Y--},
                    {'E', robot => robot._position.X++},
                };

        private static readonly IDictionary<Char, Action<Robot>> RotateCommands =
            new Dictionary<Char, Action<Robot>>
                {
                    {'L', robot => robot._orientation = robot._orientation - 1},
                    {'R', robot => robot._orientation = robot._orientation + 1}
                };

        private static readonly Char[] CardinalCompassPoints = new[] {'N', 'E', 'S', 'W'};

        internal static Robot New(String input)
        {
            var array = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (array.Length != 3)
                throw new FormatException();

            var x = Int32.Parse(array[0]);
            var y = Int32.Parse(array[1]);
            var orientation = array[2][0];

            if (Array.IndexOf(CardinalCompassPoints, orientation) == -1)
                throw new FormatException();

            return new Robot(new Point(x, y), orientation);
        }

        public override String ToString()
        {
            return String.Format("{0} {1} {2}", _position.X, _position.Y, Orientation);
        }
    }
}