using proyecto_alumnos.DTO;
using proyecto_alumnos.Repository.IRepository;
using proyecto_alumnos.Service.IService;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Service
{
    public class StudentService(IStudentRepository studentRepository) : IStudentService
    {
        private readonly IStudentRepository _studentRepository = studentRepository;
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

            if (!request.Id.HasValue || request.Id == 0) return await NewStudent(request);
            return await UpdateStudent(request);
        }
        /// <summary>
        /// Crea un nuevo alumno en la base de datos utilizando el repositorio de estudiantes. 
        /// </summary>
        /// <param name="request">Información del alumno a crear.</param>
        /// <returns>Retorna el DTO del alumno creado.</returns>
        private async Task<StudentResponseDTO> NewStudent(StudentRequestDTO request)
        {
            return await _studentRepository.CreateStudent(request);
        }

        /// <summary>
        /// Actualiza un alumno existente en la base de datos utilizando el repositorio de estudiantes.
        /// </summary>
        /// <param name="request">Información del alumno a actualizar.</param>
        /// <returns>Retorna el DTO del alumno actualizado.</returns>
        /// <exception cref="Exception"></exception>
        private async Task<StudentResponseDTO> UpdateStudent(StudentRequestDTO request)
        {
            var exists = await _studentRepository.ExistsStudent(request.Id!.Value);
            if (!exists)
            {
                throw new Exception($"El alumno con ID {request.Id} no existe para ser actualizado.");
            }

            return await _studentRepository.UpdateStudent(request);
        }
        /// <summary>
        /// Recupera una lista paginada de alumnos según los filtros proporcionados en el DTO de filtro.
        /// </summary>
        /// <param name="request">DTO con los filtros de búsqueda.</param>
        /// <returns>Retorna una lista paginada de alumnos.</returns>
        public async Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request)
        {
            return await _studentRepository.ListStudents(request);
        }
        /// <summary>
        /// Recupera una lista completa de todos los alumnos según los filtros proporcionados en el DTO de filtro.
        /// </summary>
        /// <param name="request">DTO con los filtros de búsqueda.</param>
        /// <returns>Retorna una lista completa de todos los alumnos.</returns>

        public async Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request)
        {
            return await _studentRepository.ListAllStudents(request);
        }
    }
}
