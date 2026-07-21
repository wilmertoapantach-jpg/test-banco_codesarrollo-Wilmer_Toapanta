namespace proyecto_alumnos.Shared
{
    public class PageResponseDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
