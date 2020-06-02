using System;
using System.Collections.Generic;
using System.Drawing;
using TurtleGame.Domain.Models;
using TurtleGame.Domain.Validators;
using TurtleGame.Infrastructure.Extensions;

namespace TurtleGame.Domain
{
    public class Game
    {
        private static int _sequenceCount = 1;

        private readonly GameSettings _gameSettings;
        private readonly int _boardNumberOfRows;
        private readonly int _boardNumberOfColumns;
        private readonly Point? _exitPosition;
        private readonly List<Point?> _mines;

        private const string MineHit = "Mine hit";
        private const string Success = "Success";
        private const string OutOfBoundaries = "Fell off the edge";
        private const string StillInDanger = "Still in danger";

        public Game(GameSettings gameSettings)
        {
            ValidateGameSettings(gameSettings);

            _gameSettings = gameSettings;
            _boardNumberOfRows = _gameSettings.Board.Rows;
            _boardNumberOfColumns = _gameSettings.Board.Columns;
            _exitPosition = _gameSettings.ExitPosition.Tile;
            _mines = _gameSettings.Mines;
        }

        private static void ValidateGameSettings(GameSettings gameSettings)
        {
            if (gameSettings == null)
            {
                throw new Exception("Settings file is empty.");
            }

            var gameSettingsValidator = new GameSettingsValidator();
            var results = gameSettingsValidator.Validate(gameSettings);

            if (!results.IsValid)
            {
                throw new Exception($"Game Settings file is invalid:\n{results.BuildErrorMessage()}");
            }
        }

        public string Run(TurtleAction[] actions)
        {
            var direction = (Direction)_gameSettings.StartingPosition.Direction;
            var currentPosition = (Point)_gameSettings.StartingPosition.Tile;
            var outcome = StillInDanger;

            foreach (var action in actions)
            {
                if (action == TurtleAction.Turn)
                {
                    Turn(ref direction);
                    continue;
                }
             
                MoveForward(direction, ref currentPosition);
                    
                outcome = VerifyPosition(direction, currentPosition);
                
                var isStillInDanger = outcome.Equals(StillInDanger);
                if (!isStillInDanger)
                    break;
            }

            return $"Sequence {_sequenceCount++}: {outcome}!";
        }

        private string VerifyPosition(Direction direction, Point currentPosition)
        {
            if (_mines != null && _mines.Contains(currentPosition))
            {
                return MineHit;
            }

            if (currentPosition.Equals(_exitPosition))
            {
                return Success;
            }

            if ((direction == Direction.West  && currentPosition.X < 0) ||
                (direction == Direction.North && currentPosition.Y < 0) ||
                (direction == Direction.East  && currentPosition.X == _boardNumberOfRows + 1) ||
                (direction == Direction.South && currentPosition.Y == _boardNumberOfColumns + 1))
            {
                return OutOfBoundaries;
            };

            return StillInDanger;
        }

        private void MoveForward(Direction direction, ref Point currentPosition)
        {
            if (direction.Equals(Direction.North))
            {
                currentPosition.Y--;
                return;
            }

            if (direction.Equals(Direction.East))
            {
                currentPosition.X++;
                return;
            }

            if (direction.Equals(Direction.South))
            {
                currentPosition.Y++;
                return;
            }

            if (direction.Equals(Direction.West))
            {
                currentPosition.X--;
                return;
            }
        }

        private void Turn(ref Direction direction)
        {
            direction = direction == Direction.West ? Direction.North : direction + 1;
        }
    }
}
