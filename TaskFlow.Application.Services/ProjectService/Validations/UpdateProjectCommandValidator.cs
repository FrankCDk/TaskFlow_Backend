using FluentValidation;
using TaskFlow.Application.Services.ProjectService.Command;

namespace TaskFlow.Application.Services.ProjectService.Validations
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull().WithMessage("El ID del proyecto es obligatorio.")
                .GreaterThan(0).WithMessage("El ID del proyecto debe ser mayor que 0.");

            RuleFor(x => x.ProjectName)
                .MaximumLength(100).WithMessage("El nombre del proyecto tiene un máximo de 100 caracteres.");

            RuleFor(x => x.ProjectDescription)
                .MaximumLength(500).WithMessage("La descripción del proyecto tiene un máximo de 500 caracteres.");

        }

    }
}
