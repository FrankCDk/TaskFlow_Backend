using MediatR;

namespace TaskFlow.Application.Services.ProjectService.Command
{
    public class UpdateProjectCommand : IRequest<bool>
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }        
    }
}
