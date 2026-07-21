using AutoMapper;
using proyecto_alumnos.Core.Application.DTOs;
using proyecto_alumnos.Core.Domain.Entities;

namespace proyecto_alumnos.Shared
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Student, StudentResponseDTO>().ReverseMap();
            CreateMap<Student, StudentRequestDTO>().ReverseMap();
        }
    }
}
