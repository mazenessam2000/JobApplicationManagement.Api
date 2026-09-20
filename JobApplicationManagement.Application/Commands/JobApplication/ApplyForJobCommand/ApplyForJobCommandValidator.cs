using FluentValidation;

namespace JobApplicationManagement.Application.Commands.JobApplication.ApplyForJobCommand;

public sealed class ApplyForJobCommandValidator : AbstractValidator<ApplyForJobCommand>
{
    public ApplyForJobCommandValidator()
    {
        RuleFor(x => x.JobId).NotEmpty();
        RuleFor(x => x.CoverLetter).MaximumLength(5000);
        RuleFor(x => x.ResumeUrl).MaximumLength(2048).Must(uri => uri is null || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Resume URL must be an absolute URL.");
    }
}
