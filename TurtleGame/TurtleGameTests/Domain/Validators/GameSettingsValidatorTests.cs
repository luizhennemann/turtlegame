using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using TurtleGame.Domain;
using TurtleGame.Domain.Models;
using TurtleGame.Domain.Validators;
using Xunit;

namespace TurtleGameTests.Domain.Validators
{
    public class GameSettingsValidatorTests
    {
        private readonly GameSettingsValidator _gameSettingsValidator;
        private readonly ExitPositionValidator _exitPositionValidator;
        
        public GameSettingsValidatorTests()
        {
            _gameSettingsValidator = new GameSettingsValidator();
            _exitPositionValidator = new ExitPositionValidator(5, 5);
        }

        [Fact]
        public void When_validating_game_settings_and_board_is_null_then_return_error()
        {
            var gameSettings = new GameSettings();
            
            var results = _gameSettingsValidator.Validate(gameSettings);
            
            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("Board should not be null.");
        }

        [Fact]
        public void When_validating_game_settings_and_starting_and_exit_positions_are_null_then_return_error()
        {
            var gameSettings = new GameSettings { Board = new Board { Rows = 2, Columns = 2 } };
            
            var results = _gameSettingsValidator.Validate(gameSettings);
            
            results.Errors.Count.Should().Be(2);
            results.Errors[0].ErrorMessage.Should().Be("StartingPosition should not be null.");
            results.Errors[1].ErrorMessage.Should().Be("ExitPosition should not be null.");
        }

        [Fact]
        public void When_validation_game_settings_and_starting_and_exiting_position_are_in_the_same_tile_then_return_error()
        {
            var gameSettings = new GameSettings 
            { 
                Board = new Board { Rows = 4, Columns = 5 }, 
                StartingPosition = new StartingPosition { Tile = new Point(4, 2), Direction = Direction.North },
                ExitPosition = new ExitPosition { Tile = new Point(4, 2) },
            };

            var results = _gameSettingsValidator.Validate(gameSettings);

            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("Starting and Exit position should not be in the same tile.");
        }

        [Fact]
        public void When_validating_board_and_it_is_smaller_then_two_by_two_then_return_error()
        {
            var board = new Board { Rows = 1, Columns = 1 };
            var boardValidator = new BoardValidator();
            
            var results = boardValidator.Validate(board);

            results.Errors.Count.Should().Be(2);
            results.Errors[0].ErrorMessage.Should().Be("Board rows should be greater or equal 2.");
            results.Errors[1].ErrorMessage.Should().Be("Board columns should be greater or equal 2.");
        }

        [Fact]
        public void When_validating_starting_position_and_tile_and_direction_are_null_then_return_error()
        {
            var startingPosition = new StartingPosition();
            var startingPositionValidator = new StartingPositionValidator(2, 2);

            var results = startingPositionValidator.Validate(startingPosition);

            results.Errors.Count.Should().Be(2);
            results.Errors[0].ErrorMessage.Should().Be("StartingPosition tile should not be null.");
            results.Errors[1].ErrorMessage.Should().Be("Direction should not be null.");
        }

        [Fact]
        public void When_validating_exiting_position_and_tile_is_null_then_return_error()
        {
            var exitPosition = new ExitPosition();

            var results = _exitPositionValidator.Validate(exitPosition);

            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("ExitPosition tile should not be null.");
        }

        [Fact]
        public void When_validating_exiting_position_and_tile_is_not_on_the_edge_then_return_error()
        {
            var exitPosition = new ExitPosition { Tile = new Point(2, 2) };

            var results = _exitPositionValidator.Validate(exitPosition);

            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("ExitPosition should be on the edge of the board.");
        }

        [Theory]
        [InlineData(-1, 2)]
        [InlineData(5, 2)]
        [InlineData(2, -1)]
        [InlineData(2, 5)]
        public void When_validation_tile_and_it_is_not_inside_the_board_then_return_error(int x, int y)
        {
            var parent = "Any parent";
            var tile = new Point(x, y);
            var tileValidator = new TileValidator(parent, 0, 4);

            var results = tileValidator.Validate(tile);

            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be($"Property {parent}: Tile should be inside the board.");
        }

        [Fact]
        public void When_validating_mines_and_one_of_then_is_equal_starting_or_exit_position_then_return_error()
        {
            var board = new Board { Rows = 4, Columns = 5 };
            var startingPosition = new StartingPosition { Tile = new Point(4, 2) };
            var exitPosition = new ExitPosition { Tile = new Point(4, 2) };
            var minesValidator = new MinesValidator(board, startingPosition, exitPosition);
            var mine = new Point(4, 2);

            var results = minesValidator.Validate(mine);

            results.Errors.Count.Should().Be(2);
            results.Errors[0].ErrorMessage.Should().Be("There is a mine on starting position.");
            results.Errors[1].ErrorMessage.Should().Be("There is a mine on exit position.");
        }
    }
}
