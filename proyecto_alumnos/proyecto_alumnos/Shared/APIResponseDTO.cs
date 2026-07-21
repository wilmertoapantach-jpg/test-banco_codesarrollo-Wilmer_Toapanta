using System.Net;

namespace proyecto_alumnos.Shared
{
    /// <summary>
    /// Estructura estándar para las respuestas de la API.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto de resultado.</typeparam>
    public class APIResponseDTO<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string>? Messages { get; set; }
        public T? Result { get; set; }
    }
}
