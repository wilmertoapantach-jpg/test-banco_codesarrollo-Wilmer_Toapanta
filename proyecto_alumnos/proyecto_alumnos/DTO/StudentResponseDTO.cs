namespace proyecto_alumnos.DTO
{
    public class StudentResponseDTO
    {
        /// <summary>
        /// Identificador único del alumno.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del alumno.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Estado activo/inactivo del alumno.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Descripción u observaciones del alumno.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
