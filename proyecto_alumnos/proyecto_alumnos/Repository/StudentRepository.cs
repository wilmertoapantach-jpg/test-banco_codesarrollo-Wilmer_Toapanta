using AutoMapper;
using LinqKit;
using proyecto_alumnos.DTO;
using proyecto_alumnos.Models;
using proyecto_alumnos.Repository.IRepository;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Repository
{
    public class StudentRepository(IMapper mapper) : IStudentRepository
    {
        private readonly IMapper _mapper = mapper;

        // Simulación de Base de Datos en memoria (Lista en memoria compartida)
        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, Name = "Marco Peréz", IsActive = true, Description = "Alumno con excelentes calificaciones" },
            new Student { Id = 2, Name = "Pilar Toapanta", IsActive = false, Description = "Alumno ha desertado en múltiples ocasiones" },
            new Student { Id = 3, Name = "Adrián Almeida", IsActive = true, Description = "Alumno promedio, proceso aprendizaje." },
            new Student { Id = 4, Name = "Marcela Pazmiño", IsActive = true, Description = "Alumno regular, requiere refuerzo" },
            new Student { Id = 5, Name = "Arturo Ureña", IsActive = true, Description = "Alumno regular, ha desertado en 2 ocasiones" },
            new Student { Id = 6, Name = "Lina Cachago", IsActive = false, Description = "Alumno no asiste desde segunda clase" }
        };

        private static readonly object _lock = new();


        /// <summary>
        /// Verifica si un estudiante con el ID proporcionado existe en la lista de estudiantes.
        /// </summary>
        /// <param name="id">ID del estudiante a verificar.</param>
        /// <returns>Retorna true si el estudiante existe, de lo contrario false.</returns>
        public Task<bool> ExistsStudent(int id)
        {
            lock (_lock)
            {
                var exists = _students.Any(s => s.Id == id);
                return Task.FromResult(exists);
            }
        }
        /// <summary>
        /// Crea un nuevo estudiante en la lista de estudiantes.
        /// </summary>
        /// <param name="request">Información del estudiante a crear.</param>
        /// <returns>Retorna el DTO del estudiante creado.</returns>
        public Task<StudentResponseDTO> CreateStudent(StudentRequestDTO request)
        {
            lock (_lock)
            {
                var entity = _mapper.Map<Student>(request);

                // Generar nuevo ID autoincremental
                int newId = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;
                entity.Id = newId;
                _students.Add(entity);
                var response = _mapper.Map<StudentResponseDTO>(entity);
                return Task.FromResult(response);
            }
        }
        /// <summary>
        /// Actualiza la información de un estudiante existente en la lista de estudiantes.
        /// </summary>
        /// <param name="request">Información del estudiante a actualizar.</param>
        /// <returns>Retorna el DTO del estudiante actualizado.</returns>
        /// <exception cref="Exception"></exception>
        public Task<StudentResponseDTO> UpdateStudent(StudentRequestDTO request)
        {
            lock (_lock)
            {
                var existing = _students.FirstOrDefault(s => s.Id == request.Id);
                if (existing is null)
                {
                    throw new Exception($"El alumno con ID {request.Id} no existe.");
                }

                existing.Name = request.Name;
                existing.IsActive = request.IsActive;
                existing.Description = request.Description;

                var response = _mapper.Map<StudentResponseDTO>(existing);
                return Task.FromResult(response);
            }
        }
        /// <summary>
        /// Retorna una lista paginada de estudiantes según los filtros proporcionados en el objeto StudentFilterDTO.
        /// </summary>
        /// <param name="request">Objeto con los filtros de búsqueda.</param>
        /// <returns>Retorna una lista paginada de estudiantes.</returns>
        public Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request)
        {
            lock (_lock)
            {
                IEnumerable<Student> query = _students;

                var predicate = PredicateBuilder.New<Student>(true);
                if (request.Id.HasValue && request.Id > 0) predicate = predicate.And(s => s.Id == request.Id.Value);
                if (!string.IsNullOrWhiteSpace(request.Name)) predicate = predicate.And(s => s.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                if (request.IsActive.HasValue) predicate = predicate.And(s => s.IsActive == request.IsActive.Value);

                var count = query.Count();
                var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
                var pageSize = request.PageSize > 0 ? request.PageSize : 10;

                var items = query.Where(predicate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var pageResponse = new PageResponseDTO<StudentResponseDTO>
                {
                    Items = _mapper.Map<List<StudentResponseDTO>>(items),
                    Count = count,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return Task.FromResult(pageResponse);
            }
        }
        /// <summary>
        /// Recupera todos los estudiantes según los filtros proporcionados en el objeto StudentAllFilterDTO.
        /// </summary>
        /// <param name="request">Objeto con los filtros de búsqueda.</param>
        /// <returns>Retorna una lista completa de todos los estudiantes.</returns>
        public Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request)
        {
            lock (_lock)
            {
                IEnumerable<Student> query = _students;
                var predicate = PredicateBuilder.New<Student>(true);

                if (!string.IsNullOrWhiteSpace(request.Name)) predicate = predicate.And(s => s.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                if (request.IsActive.HasValue) predicate = predicate.And(s => s.IsActive == request.IsActive.Value);

                var items = query.Where(predicate).ToList();
                var result = _mapper.Map<List<StudentResponseDTO>>(items);
                return Task.FromResult(result);
            }
        }
    }
}
