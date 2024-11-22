using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Entities;

namespace PruebaIngresoBibliotecario.Core.Abstractions.Repotories;
public interface IPrestamoRepository
{
    Task<Prestamo> IngresarPrestamo(Prestamo prestamo);
    Task<Prestamo?> Consultar(Guid idPrestamo);
    Task<bool> ConsultarPrestamoUsuarioInvitado(string identificacionUsuario);
}
