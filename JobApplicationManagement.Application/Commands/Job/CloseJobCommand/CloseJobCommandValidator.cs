using FluentValidation;

namespace JobApplicationManagement.Application.Commands.Job.CloseJobCommand;

public sealed class CloseJobCommandValidator : AbstractValidator<CloseJobCommand>
{
    public CloseJobCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}
