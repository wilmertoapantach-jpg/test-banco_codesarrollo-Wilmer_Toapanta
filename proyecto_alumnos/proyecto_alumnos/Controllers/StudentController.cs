using Microsoft.AspNetCore.Mvc;
using proyecto_alumnos.Core.Application.DTOs;
using proyecto_alumnos.Core.Application.Ports.In;
using proyecto_alumnos.Shared;
using System.Net;

namespace proyecto_alumnos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;

        /// <summary>
        /// Crea o actualiza un alumno según el valor del campo <c>Id</c>.
        /// Si <c>Id</c> es 0 o null: crea un nuevo alumno con ID autoincremental.
        /// Si <c>Id</c> es mayor a 0: actualiza el alumno existente.
        /// </summary>
        [HttpPost("SaveStudent")]
        public async Task<ActionResult<APIResponseDTO<StudentResponseDTO>>> SaveStudent([FromBody] StudentRequestDTO request)
        {
            try
            {
                var result = await _studentService.SaveStudent(request);
                return Ok(new APIResponseDTO<StudentResponseDTO>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponseDTO<StudentResponseDTO>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Messages = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Retorna una lista paginada de alumnos con filtros opcionales.
        /// </summary>
        [HttpPost("ListStudents")]
        public async Task<ActionResult<APIResponseDTO<PageResponseDTO<StudentResponseDTO>>>> ListStudents([FromBody] StudentFilterDTO request)
        {
            try
            {
                var result = await _studentService.ListStudents(request);
                return Ok(new APIResponseDTO<PageResponseDTO<StudentResponseDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponseDTO<PageResponseDTO<StudentResponseDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Messages = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Retorna la lista completa de alumnos sin paginación, aplicando filtros opcionales.
        /// </summary>
        [HttpPost("ListAllStudents")]
        public async Task<ActionResult<APIResponseDTO<List<StudentResponseDTO>>>> ListAllStudents([FromBody] StudentAllFilterDTO request)
        {
            try
            {
                var result = await _studentService.ListAllStudents(request);
                return Ok(new APIResponseDTO<List<StudentResponseDTO>>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponseDTO<List<StudentResponseDTO>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Messages = new List<string> { ex.Message }
                });
            }
        }
    }
}
