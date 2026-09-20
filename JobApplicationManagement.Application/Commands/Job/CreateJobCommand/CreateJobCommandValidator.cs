using FluentValidation;

namespace JobApplicationManagement.Application.Commands.Job.CreateJobCommand;

public class CreateJobCommandValidator
    : AbstractValidator<CreateJobCommand>
{
    public CreateJobCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SalaryMin.HasValue);

        RuleFor(x => x.SalaryMax)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SalaryMax.HasValue);

        RuleFor(x => x)
            .Must(x =>
                !x.SalaryMin.HasValue ||
                !x.SalaryMax.HasValue ||
                x.SalaryMin <= x.SalaryMax)
            .WithMessage("Minimum salary cannot be greater than maximum salary.");
    }
}