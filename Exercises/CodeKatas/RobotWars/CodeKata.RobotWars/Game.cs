using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeKata.RobotWars
{
    public class Game
    {
        private readonly Point _boardSize;
        private readonly IList<Robot> _robots = new List<Robot>();

        public Game(Int32 width, Int32 height)
        {
            _boardSize = new Point(width, height);
        }

        public Point BoardSize
        {
            get { return _boardSize; }
        }

        public void WithRobots(Action<Robot[]> action)
        {
            action(_robots.ToArray());
        }

        public void Take(String input)
        {
            CommandFactory.Build(input).Execute(this);
        }

        internal void AddRobot(Robot robot)
        {
            _robots.Add(robot);
        }

        internal void RotateRobot(Char input)
        {
            _robots.Last().Turn(input);
        }

        internal void MoveRobot(Char input)
        {
            _robots.Last().Move(input);
        }
    }
}