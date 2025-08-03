using MediatR;

namespace TaskFlow.Application.Services.ProjectService.Command
{
    public class CreateProjectCommand : IRequest<bool>
    {
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
    }
}
