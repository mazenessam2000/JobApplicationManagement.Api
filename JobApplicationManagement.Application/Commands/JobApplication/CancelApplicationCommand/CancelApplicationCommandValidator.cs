using FluentValidation;

namespace JobApplicationManagement.Application.Commands.JobApplication.CancelApplicationCommand;

public sealed class CancelApplicationCommandValidator : AbstractValidator<CancelApplicationCommand>
{
    public CancelApplicationCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}
