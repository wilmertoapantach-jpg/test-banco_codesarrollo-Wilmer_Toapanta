using AutoMapper;
using proyecto_alumnos.DTO;
using proyecto_alumnos.Models;
using proyecto_alumnos.Repository.IRepository;
using proyecto_alumnos.Shared;

namespace proyecto_alumnos.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IMapper _mapper;

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

        public StudentRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<bool> ExistsStudent(int id)
        {
            lock (_lock)
            {
                var exists = _students.Any(s => s.Id == id);
                return Task.FromResult(exists);
            }
        }

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

        public Task<PageResponseDTO<StudentResponseDTO>> ListStudents(StudentFilterDTO request)
        {
            lock (_lock)
            {
                IEnumerable<Student> query = _students;

                if (request.Id.HasValue && request.Id > 0)
                {
                    query = query.Where(s => s.Id == request.Id.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(s => s.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (request.IsActive.HasValue)
                {
                    query = query.Where(s => s.IsActive == request.IsActive.Value);
                }

                var count = query.Count();
                var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
                var pageSize = request.PageSize > 0 ? request.PageSize : 10;

                var items = query
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

        public Task<List<StudentResponseDTO>> ListAllStudents(StudentAllFilterDTO request)
        {
            lock (_lock)
            {
                IEnumerable<Student> query = _students;

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(s => s.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (request.IsActive.HasValue)
                {
                    query = query.Where(s => s.IsActive == request.IsActive.Value);
                }

                var items = query.ToList();
                var result = _mapper.Map<List<StudentResponseDTO>>(items);
                return Task.FromResult(result);
            }
        }
    }
}
