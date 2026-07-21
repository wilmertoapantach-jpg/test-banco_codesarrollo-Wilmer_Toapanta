using proyecto_alumnos.Core.Application.DTOs;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Core.Application.Ports.In
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> SaveStudent(StudentRequestDTO request);
        Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request);
        Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request);
    }
}
