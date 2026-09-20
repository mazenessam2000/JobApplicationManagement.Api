using FluentValidation;
using JobApplicationManagement.Domain.Enums;

namespace JobApplicationManagement.Application.Commands.JobApplication.UpdateApplicationStatusCommand;

public sealed class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatusCommand>
{
    public UpdateApplicationStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum().NotEqual(ApplicationStatus.Submitted)
            .WithMessage("Recruiters cannot set an application back to Submitted.");
    }
}
