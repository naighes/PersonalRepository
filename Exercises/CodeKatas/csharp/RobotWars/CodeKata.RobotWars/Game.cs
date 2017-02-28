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
            action(_robots.Select(r => Robot.New(r.ToString())).ToArray());
        }

        public void Take(String input)
        {
            InputCommands.First(command => command.Key(input)).Value(_robots, input);
        }

        private static readonly IDictionary<Predicate<String>, Action<IList<Robot>, String>> InputCommands = 
            new Dictionary<Predicate<String>, Action<IList<Robot>, String>>
                {
                    {
                        input => Char.IsNumber(input[0]), 
                        (robots, input) => robots.Add(Robot.New(input))
                    },
                    {
                        input => true, 
                        (robots, input) => input.ToList().ForEach(c => TransformationCommands[c](robots.Last()))
                    }
                };

        private static readonly IDictionary<Char, Action<Robot>> TransformationCommands =
            new Dictionary<Char, Action<Robot>>
                {
                    {'L', robot => robot.TurnLeft()},
                    {'R', robot => robot.TurnRight()},
                    {'M', robot => robot.Move()}
                };
    }
}