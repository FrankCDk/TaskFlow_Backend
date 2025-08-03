using MediatR;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Services.ProjectService.Command;

namespace TaskFlow.Application.Services.ProjectService.Handler
{
    public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, bool>
    {
        private readonly IProjectRepository _projectRepository;
        public UpdateProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
