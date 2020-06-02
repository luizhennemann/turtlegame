using System.Collections.Generic;
using FluentValidation;

namespace TurtleGame.Domain.Validators
{
    public class SequencesValidator : AbstractValidator<List<TurtleAction[]>>
    {
        public SequencesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(list => list)
                .Must(list => list.Count > 0).WithMessage("At least one sequence should be informed on moves file.");

            When(list => list.Count > 0, () =>
            {
                RuleForEach(seq => seq).Must(a => a.Length > 0).WithMessage("All sequences should have at least one action.");
            });
        }
    }
}
