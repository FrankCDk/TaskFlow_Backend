using AutoMapper;
using TaskFlow.Application.Services.ProjectService.Command;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services.ProjectService.Mapper
{
    public class ProjectProfile : Profile
    {

        public ProjectProfile()
        {
            // Mapping configuration for CreateProjectCommand to Project entity
            CreateMap<CreateProjectCommand, Project>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProjectDescription))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }



    }
}
