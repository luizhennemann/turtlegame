using System.Drawing;
using FluentValidation;
using TurtleGame.Domain.Models;

namespace TurtleGame.Domain.Validators
{
    public class GameSettingsValidator : AbstractValidator<GameSettings>
    {
        public GameSettingsValidator()
        {
            RuleFor(set => set.Board)
                .NotNull().WithMessage("Board should not be null.")
                .SetValidator(new BoardValidator());

            When(set => set.Board?.Rows >= 2 && set.Board?.Columns >= 2, () =>
            {
                RuleFor(set => set.StartingPosition)
                    .NotNull().WithMessage("StartingPosition should not be null.")
                    .SetValidator(set => new StartingPositionValidator(set.Board.Rows, set.Board.Columns));
            
                RuleFor(set => set.ExitPosition)
                    .NotNull().WithMessage("ExitPosition should not be null.")
                    .SetValidator(set => new ExitPositionValidator(set.Board.Rows, set.Board.Columns));

                When(set => set.StartingPosition?.Tile != null && set.ExitPosition?.Tile != null, () => 
                {
                    RuleFor(set => set.StartingPosition.Tile).NotEqual(set => set.ExitPosition.Tile).WithMessage("Starting and Exit position should not be in the same tile.");
                    RuleForEach(set => set.Mines).SetValidator(set => new MinesValidator(set.Board, set.StartingPosition, set.ExitPosition)); 
                });
            });
        }
    }

    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(b => b.Rows).GreaterThanOrEqualTo(2).WithMessage("Board rows should be greater or equal 2.");
            RuleFor(b => b.Columns).GreaterThanOrEqualTo(2).WithMessage("Board columns should be greater or equal 2.");
        }
    }

    public class StartingPositionValidator : AbstractValidator<StartingPosition>
    {
        public StartingPositionValidator(int rows, int columns)
        {
            RuleFor(sp => sp.Tile)
                .NotNull().WithMessage("StartingPosition tile should not be null.")
                .SetValidator(new TileValidator("StartingPosition", rows, columns));
            RuleFor(sp => sp.Direction)
                .NotNull().WithMessage("Direction should not be null.");
        }
    }

    public class ExitPositionValidator : AbstractValidator<ExitPosition>
    {
        public ExitPositionValidator(int rows, int columns)
        {
            RuleFor(ep => ep.Tile)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("ExitPosition tile should not be null.")
                .SetValidator(new TileValidator("ExitPosition", rows, columns))
                .Must(tile => BeOnTheEdge(tile, rows, columns)).WithMessage("ExitPosition should be on the edge of the board."); ;
        }

        private bool BeOnTheEdge(Point? tile, int rows, int columns)
        {
            var newTile = (Point)tile;
            return newTile.X == 0 || newTile.Y == 0 || newTile.X == columns - 1 || newTile.Y == rows - 1;
        }
    }

    public class MinesValidator : AbstractValidator<Point?>
    {
        public MinesValidator(Board board, StartingPosition startingPosition, ExitPosition exitPosition)
        {
            RuleFor(m => m)
                .NotEqual(startingPosition.Tile).WithMessage($"There is a mine on starting position.")
                .NotEqual(exitPosition.Tile).WithMessage($"There is a mine on exit position.")
                .SetValidator(m => new TileValidator($"Mines {m.GetValueOrDefault()}", board.Rows, board.Columns)).WithMessage("Some of the mines are invalid.");
        }
    }

    public class TileValidator : AbstractValidator<Point?>
    {
        public TileValidator(string parent, int rows, int columns)
        {
            RuleFor(tile => tile)
                .Must(tile => BeInsideTheBoard(tile, rows, columns)).WithMessage($"Property {parent}: Tile should be inside the board.");

        }

        private bool BeInsideTheBoard(Point? tile, int rows, int columns)
        {
            var newTile = (Point)tile;
            return newTile.X >= 0 && newTile.X <= columns && newTile.Y >= 0 && newTile.Y < rows;
        }
    }
}
