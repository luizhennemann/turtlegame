using System.Collections.Generic;
using FluentAssertions;
using FluentValidation.Results;
using TurtleGame.Infrastructure.Extensions;
using Xunit;

namespace TurtleGameTests.Infrastructure
{
    public class ValidationResultExtensionsTests
    {
        [Fact]
        public void When_building_error_message_then_return_a_message()
        {
            var firstMessage = "First message";
            var secondMessage = "Second message";

            var failures = new List<ValidationFailure> { new ValidationFailure("", firstMessage), new ValidationFailure("", secondMessage) };
            var results = new ValidationResult(failures);

            var errorMessage = results.BuildErrorMessage();
            errorMessage.Should().Be($"{firstMessage}\r\n{secondMessage}\r\n");
        }
    }
}
