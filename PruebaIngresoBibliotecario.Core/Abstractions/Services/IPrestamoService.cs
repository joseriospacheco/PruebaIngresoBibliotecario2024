using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Entities;

namespace PruebaIngresoBibliotecario.Core.Abstractions.Services;
public interface IPrestamoService
{
    Task<RespuestaPrestamoExitosoDTO> IngresarPrestamo(CrearPrestamoDTO crearPrestamoDTO);
    Task<ConsultaPrestamoTO> Consultar(Guid idPrestamo);
    Task<bool> ConsultarPrestamoUsuarioInvitado(string identificacionUsuario);

}
