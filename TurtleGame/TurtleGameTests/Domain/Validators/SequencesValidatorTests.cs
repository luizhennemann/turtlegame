using System.Collections.Generic;
using FluentAssertions;
using TurtleGame.Domain;
using TurtleGame.Domain.Validators;
using Xunit;

namespace TurtleGameTests.Domain.Validators
{
    public class SequencesValidatorTests
    {
        private readonly SequencesValidator _sut;

        public SequencesValidatorTests()
        {
            _sut = new SequencesValidator();
        }

        [Fact]
        public void When_validating_sequences_and_file_is_empty_then_return_error()
        {
            var sequences = new List<TurtleAction[]> { };
            var results = _sut.Validate(sequences);
            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("At least one sequence should be informed on moves file.");
        }

        [Fact]
        public void When_validating_sequences_and_one_of_the_sequences_does_not_contain_actions_then_return_error()
        {
            var sequences = new List<TurtleAction[]> { new TurtleAction[] { TurtleAction.Move, TurtleAction.Turn }, new TurtleAction[] { } };
            var results = _sut.Validate(sequences);
            results.Errors.Count.Should().Be(1);
            results.Errors[0].ErrorMessage.Should().Be("All sequences should have at least one action.");
        }
    }
}
