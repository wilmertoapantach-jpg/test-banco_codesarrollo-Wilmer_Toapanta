using proyecto_alumnos.Core.Application.DTOs;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Core.Application.Ports.Out
{
    public interface IStudentRepository
    {
        Task<StudentResponseDTO> CreateStudent(StudentRequestDTO request);
        Task<StudentResponseDTO> UpdateStudent(StudentRequestDTO request);
        Task<bool> ExistsStudent(int id);
        Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request);
        Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request);
    }
}
