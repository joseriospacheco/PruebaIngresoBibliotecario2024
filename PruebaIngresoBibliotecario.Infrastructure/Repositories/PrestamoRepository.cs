
using Microsoft.EntityFrameworkCore;
using PruebaIngresoBibliotecario.Core.Abstractions.Repotories;
using PruebaIngresoBibliotecario.Core.Entities;
using PruebaIngresoBibliotecario.Core.Enumerations;

namespace PruebaIngresoBibliotecario.Infrastructure.Repositories;
public class PrestamoRepository : IPrestamoRepository
{

    private readonly PersistenceContext _persistenceContext;

    public PrestamoRepository(PersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async Task<Prestamo?> Consultar(Guid idPrestamo)
    {
        return await _persistenceContext.Prestamos.FindAsync(idPrestamo);
    }

    public async Task<bool> ConsultarPrestamoUsuarioInvitado(string identificacionUsuario)
    {
        return await _persistenceContext.Prestamos.AsNoTracking().AnyAsync(x => x.IdentificacionUsuario.Equals(identificacionUsuario) && x.TipoUsuario == (int)TipoUsuarioPrestamo.INVITADO);
    }

    public async Task<Prestamo> IngresarPrestamo(Prestamo prestamo)
    {
        await _persistenceContext.AddAsync(prestamo);
        await _persistenceContext.SaveChangesAsync();
        return prestamo;
    }
}
