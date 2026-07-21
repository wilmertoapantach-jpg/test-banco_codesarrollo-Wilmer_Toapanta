namespace proyecto_alumnos.DTO
{
    public class StudentFilterDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
