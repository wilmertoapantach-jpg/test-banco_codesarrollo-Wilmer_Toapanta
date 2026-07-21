using AutoMapper;
using proyecto_alumnos.DTO;
using proyecto_alumnos.Models;

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
