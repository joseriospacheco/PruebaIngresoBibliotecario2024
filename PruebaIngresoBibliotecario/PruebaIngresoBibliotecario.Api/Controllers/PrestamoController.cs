using Microsoft.AspNetCore.Mvc;
using PruebaIngresoBibliotecario.Core.Abstractions.Services;
using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Enumerations;
using System;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        public PrestamoController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrearPrestamoDTO dto)
        {
            try
            {
                if (!Enum.IsDefined(typeof(TipoUsuarioPrestamo), dto.TipoUsuario))
                    return BadRequest("Error en los datos de entrada");

                var cantidadPrestamos = await _prestamoService.ConsultarPrestamoUsuarioInvitado(dto.IdentificacionUsuario);

                if (cantidadPrestamos)

                    return NotFound(new { mensaje = $"El usuario con identificacion {dto.IdentificacionUsuario} ya tiene un libro prestado por lo cual no se le puede realizar otro prestamo" });

                var prestamo = await _prestamoService.IngresarPrestamo(dto);
                return Ok(prestamo);

            }
            catch
            {
                return BadRequest("Error en los datos de entrada");
            }
        }

        [HttpGet]
        [Route("{idPrestamo}")]
        public async Task<ActionResult> Get(string idPrestamo)
        {
            try
            {
                var prestamo = await _prestamoService.Consultar(Guid.Parse(idPrestamo));

                if (prestamo is not null)
                    return Ok(prestamo);

                return NotFound(new { mensaje = $"El prestamo con id {idPrestamo} no existe" });
            }
            catch
            {
                return BadRequest("Error en los datos de entrada");
            }
        }
    }
}
