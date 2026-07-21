using proyecto_alumnos.DTO;
using proyecto_alumnos.Repository.IRepository;
using proyecto_alumnos.Service.IService;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Determina si se debe crear un nuevo alumno o actualizar uno existente según el valor del campo <c>Id</c>.
        /// Si <c>Id</c> es nulo o 0, crea un nuevo alumno autoincrementando el ID.
        /// Si <c>Id</c> es mayor a 0, actualiza el registro existente.
        /// </summary>
        public async Task<StudentResponseDTO> SaveStudent(StudentRequestDTO request)
        {
            if (request is null)
                throw new Exception("La información del alumno no puede ser nula.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("El nombre del alumno es obligatorio.");

            if (!request.Id.HasValue || request.Id == 0)
            {
                return await NewStudent(request);
            }
            else
            {
                return await UpdateStudent(request);
            }
        }

        private async Task<StudentResponseDTO> NewStudent(StudentRequestDTO request)
        {
            return await _studentRepository.CreateStudent(request);
        }

        private async Task<StudentResponseDTO> UpdateStudent(StudentRequestDTO request)
        {
            var exists = await _studentRepository.ExistsStudent(request.Id!.Value);
            if (!exists)
            {
                throw new Exception($"El alumno con ID {request.Id} no existe para ser actualizado.");
            }

            return await _studentRepository.UpdateStudent(request);
        }

        public async Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request)
        {
            return await _studentRepository.ListStudents(request);
        }

        public async Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request)
        {
            return await _studentRepository.ListAllStudents(request);
        }
    }
}
