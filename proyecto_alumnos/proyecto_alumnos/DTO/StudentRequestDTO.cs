namespace proyecto_alumnos.DTO
{
    public class StudentRequestDTO
    {
        /// <summary>
        /// ID del alumno. Si es 0 o null se crea un nuevo registro; si es mayor a 0 se actualiza.
        /// </summary>
        public int? Id { get; set; }

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
