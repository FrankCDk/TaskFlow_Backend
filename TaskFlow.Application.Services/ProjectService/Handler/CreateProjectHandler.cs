using AutoMapper;
using MediatR;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Services.ProjectService.Command;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services.ProjectService.Handler
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, bool>
    {
        private readonly IProjectRepository _proyectRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly IMapper _mapper;
        public CreateProjectHandler(
            IProjectRepository proyectRepository, 
            ILogsRepository logsRepository,
            IMapper mapper)
        {
            _proyectRepository = proyectRepository;
            _logsRepository = logsRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Project>(request);

            var result = await _proyectRepository.CreateAsync(project, cancellationToken);

            if(result == null)
            {
                throw new Exception("Error al registrar el proyecto.");
            }

            return true;
        }
    }
}
