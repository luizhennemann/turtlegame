using System.Text;
using FluentValidation.Results;

namespace TurtleGame.Infrastructure.Extensions
{
    public static class ValidationResultExtensions
    {
        public static string BuildErrorMessage(this ValidationResult results)
        {
            var errorMessage = new StringBuilder();

            foreach (var failure in results.Errors)
            {
                errorMessage.AppendLine(failure.ErrorMessage);
            }

            return errorMessage.ToString();
        }
    }
}
