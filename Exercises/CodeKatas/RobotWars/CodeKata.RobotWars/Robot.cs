using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeKata.RobotWars
{
    public partial class Robot
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
            get { return CardinalCompassPoints[_orientation & (CardinalCompassPoints.Length - 1)]; }
        }
        private Int32 _orientation;

        public void TurnLeft()
        {
            Interlocked.Decrement(ref _orientation);
        }

        public void TurnRight()
        {
            Interlocked.Increment(ref _orientation);
        }

        public void Move()
        {
            MovementRouteCommands[Orientation](this);
        }

        private static readonly IDictionary<Char, Action<Robot>> MovementRouteCommands =
            new Dictionary<Char, Action<Robot>>
                {
                    {'N', robot => Interlocked.Increment(ref robot._position.Y)},
                    {'W', robot => Interlocked.Decrement(ref robot._position.X)},
                    {'S', robot => Interlocked.Decrement(ref robot._position.Y)},
                    {'E', robot => Interlocked.Increment(ref robot._position.X)}
                };

        private static readonly Char[] CardinalCompassPoints = new[] {'N', 'E', 'S', 'W'};

        public override String ToString()
        {
            return String.Format("{0} {1} {2}", _position.X, _position.Y, Orientation);
        }
    }
}