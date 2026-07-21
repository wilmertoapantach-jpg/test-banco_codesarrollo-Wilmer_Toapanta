namespace proyecto_alumnos.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
