using System.Collections.Generic;
using System.Drawing;
using TurtleGame.Domain;
using Xunit;
using FluentAssertions;
using System;
using TurtleGame.Domain.Models;

namespace TurtleGameTests.Domain
{
    public class GameTests
    {
        private readonly Game _sut;

        private const string MineHit = "Mine hit";
        private const string Success = "Success";
        private const string OutOfBoundaries = "Fell off the edge";
        private const string StillInDanger = "Still in danger";

        private static int _sequenceCount = 1;

        public static IEnumerable<object[]> GameData => new[]
        {
            new object[] { new TurtleAction[] { TurtleAction.Move, TurtleAction.Turn, TurtleAction.Move, TurtleAction.Move, TurtleAction.Move, TurtleAction.Move, TurtleAction.Turn, TurtleAction.Move, TurtleAction.Move } , Success },
            new object[] { new TurtleAction[] { TurtleAction.Turn, TurtleAction.Move } , MineHit },
            new object[] { new TurtleAction[] { TurtleAction.Turn, TurtleAction.Turn, TurtleAction.Turn }, StillInDanger },
            new object[] { new TurtleAction[] { TurtleAction.Move, TurtleAction.Move } , OutOfBoundaries },
        };

        public static IEnumerable<object[]> GameSettingsData => new[]
        {
            new object[] { null, "Settings file is empty." },
            new object[] { new GameSettings(), "Game Settings file is invalid:\nBoard should not be null." },
        };

        public GameTests()
        {
            var gameSettings = new GameSettings
            {
                Board = new Board { Rows = 4, Columns = 5 },
                StartingPosition = new StartingPosition { Tile = new Point(0, 1), Direction = Direction.North },
                ExitPosition = new ExitPosition { Tile = new Point(4, 2) },
                Mines = new List<Point?> { new Point(1, 1), new Point(3, 1), new Point(3, 3) }
            };

            _sut = new Game(gameSettings);
        }

        [Theory, MemberData(nameof(GameData))]
        public void When_executing_program_and_file_names_are_invalid_then_do_not_run_the_game(TurtleAction[] actions, string outcome)
        {
            var resultMessage = $"Sequence {_sequenceCount++}: {outcome}!";
            var result = _sut.Run(actions);
            result.Should().Be(resultMessage);
        }

        [Theory, MemberData(nameof(GameSettingsData))]
        public void When_newing_up_game_and_validations_not_pass_then_return_exception(GameSettings gameSettings, string expectedMessage)
        {
            Action act = () => new Game(gameSettings);
            act.Should().Throw<Exception>().WithMessage(expectedMessage);
        }
    }
}
