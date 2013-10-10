using System;
using System.Collections.Generic;

namespace CodeKata.RobotWars
{
    internal static class CommandFactory
    {
        internal static Command<String> Build(String input)
        {
            if (Char.IsNumber(input[0]))
                return new PlacementCommand(input);

            return new TransformationSetCommand(input);
        }

        internal static Command<Char> Build(Char input)
        {
            return TransformationCommands[input](input);
        }

        private static readonly IDictionary<Char, Func<Char, Command<Char>>> TransformationCommands =
            new Dictionary<Char, Func<Char, Command<Char>>>
                {
                    {'L', i => new RotationCommand(i)},
                    {'R', i => new RotationCommand(i)},
                    {'M', i => new MovementCommand(i)}
                };
    }
}