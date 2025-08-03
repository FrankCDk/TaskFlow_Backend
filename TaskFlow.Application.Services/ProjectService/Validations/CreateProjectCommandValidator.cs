using FluentValidation;
using TaskFlow.Application.Services.ProjectService.Command;

namespace TaskFlow.Application.Services.ProjectService.Validations
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {

        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectName)
                .NotNull().WithMessage("El nombre del proyecto es obligatorio.")
                .NotEmpty().WithMessage("El nombre del proyecto no puede estar vacio.")
                .MaximumLength(100).WithMessage("El nombre del proyecto tiene un máximo de 100 caracteres.");

            RuleFor(x => x.ProjectDescription)
                .NotNull().WithMessage("La descripción del proyecto es obligatorio.")
                .NotEmpty().WithMessage("La descripción del proyecto no puede estar vacio.")
                .MaximumLength(500).WithMessage("La descripción del proyecto tiene un máximo de 500 caracteres.");
        }



    }
}
