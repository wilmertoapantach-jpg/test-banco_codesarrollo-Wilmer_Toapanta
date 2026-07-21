using proyecto_alumnos.DTO;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Service.IService
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> SaveStudent(StudentRequestDTO request);
        Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request);
        Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request);
    }
}
