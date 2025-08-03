using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Services;
using TaskFlow.Application.Services.ProjectService.Mapper;
using TaskFlow.Application.Services.ProjectService.Validations;

namespace TaskFlow.Application.Configuration
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProjectProfile>();
            });

            services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }




    }
}
