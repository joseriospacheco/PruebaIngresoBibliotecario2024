using AutoMapper;
using PruebaIngresoBibliotecario.Core.Abstractions.Repotories;
using PruebaIngresoBibliotecario.Core.Abstractions.Services;
using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Entities;
using PruebaIngresoBibliotecario.Core.Enumerations;

namespace PruebaIngresoBibliotecario.Core.Services;
public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IMapper _mapper;

    public PrestamoService(IPrestamoRepository prestamoRepository, IMapper mapper)
    {
        _prestamoRepository = prestamoRepository;
        _mapper = mapper;
    }

    public async Task<ConsultaPrestamoTO> Consultar(Guid idPrestamo)
    {
        var prestamo = await _prestamoRepository.Consultar(idPrestamo);
        return _mapper.Map<ConsultaPrestamoTO>(prestamo);

    }

    public async Task<bool> ConsultarPrestamoUsuarioInvitado(string identificacionUsuario)
    {
        return await _prestamoRepository.ConsultarPrestamoUsuarioInvitado(identificacionUsuario);
    }

    public async Task<RespuestaPrestamoExitosoDTO> IngresarPrestamo(CrearPrestamoDTO crearPrestamoDTO)
    {
        var prestamo = _mapper.Map<Prestamo>(crearPrestamoDTO);
        prestamo.FechaMaximaDevolucion = CalcularFechaEntrega((TipoUsuarioPrestamo)prestamo.TipoUsuario, DateTime.Now);
        prestamo = await _prestamoRepository.IngresarPrestamo(prestamo);
        return _mapper.Map<RespuestaPrestamoExitosoDTO>(prestamo);
    }
    public static DateTime CalcularFechaEntrega(TipoUsuarioPrestamo tipoUsuario, DateTime fechaDevolucion)
    {
        var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

        int diasPrestamo = tipoUsuario switch
        {
            TipoUsuarioPrestamo.AFILIADO => 10,
            TipoUsuarioPrestamo.EMPLEADO => 8,
            TipoUsuarioPrestamo.INVITADO => 7,
            _ => -1,
        };

        for (int i = 0; i < diasPrestamo;)
        {
            fechaDevolucion = fechaDevolucion.AddDays(1);
            i = (!weekend.Contains(fechaDevolucion.DayOfWeek)) ? ++i : i;
        }
        return fechaDevolucion;
    }
}
