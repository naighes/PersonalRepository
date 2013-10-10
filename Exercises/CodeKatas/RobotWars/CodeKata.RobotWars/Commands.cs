using System;

namespace CodeKata.RobotWars
{
    internal class TransformationSetCommand : Command<String>
    {
        public TransformationSetCommand(String input)
            : base(input)
        {
        }

        public override void Execute(Game game)
        {
            foreach (var movement in Input)
                CommandFactory.Build(movement).Execute(game);
        }
    }

    internal class PlacementCommand : Command<String>
    {
        public PlacementCommand(String input)
            : base(input)
        {
        }

        public override void Execute(Game game)
        {
            game.AddRobot(Robot.New(Input));
        }
    }

    internal class RotationCommand : Command<Char>
    {
        public RotationCommand(Char input)
            : base(input)
        {
        }

        public override void Execute(Game game)
        {
            game.RotateRobot(Input);
        }
    }

    internal class MovementCommand : Command<Char>
    {
        public MovementCommand(Char input)
            : base(input)
        {
        }

        public override void Execute(Game game)
        {
            game.MoveRobot(Input);
        }
    }

    internal abstract class Command<T>
    {
        protected readonly T Input;

        protected Command(T input)
        {
            Input = input;
        }

        public abstract void Execute(Game game);
    }
}