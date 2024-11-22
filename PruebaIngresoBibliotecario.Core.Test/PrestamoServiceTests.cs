using AutoMapper;
using PruebaIngresoBibliotecario.Core.Abstractions.Repotories;
using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Entities;
using PruebaIngresoBibliotecario.Core.Services;
using PruebaIngresoBibliotecario.Core.Enumerations;
using Moq;
namespace PruebaIngresoBibliotecario.Core.Test;
public class PrestamoServiceTests
{
    private readonly Mock<IPrestamoRepository> _mockPrestamoRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PrestamoService _prestamoService;

    public PrestamoServiceTests()
    {
        _mockPrestamoRepository = new Mock<IPrestamoRepository>();
        _mockMapper = new Mock<IMapper>();
        _prestamoService = new PrestamoService(_mockPrestamoRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task IngresarPrestamoExitoso()
    {
        var crearPrestamoDto = new CrearPrestamoDTO
        {
            IdentificacionUsuario = "110872718",
            Isbn = Guid.NewGuid().ToString(),
            TipoUsuario = (int)TipoUsuarioPrestamo.INVITADO
        };

        var prestamo = new Prestamo { Id = Guid.NewGuid(), IdentificacionUsuario = crearPrestamoDto.IdentificacionUsuario };
        var expectedResponse = new RespuestaPrestamoExitosoDTO { Id = prestamo.Id, FechaMaximaDevolucion = DateTime.Now.AddDays(7) };

        _mockMapper.Setup(mapper => mapper.Map<Prestamo>(crearPrestamoDto)).Returns(prestamo);
        _mockPrestamoRepository.Setup(repo => repo.IngresarPrestamo(prestamo)).ReturnsAsync(prestamo);
        _mockMapper.Setup(mapper => mapper.Map<RespuestaPrestamoExitosoDTO>(prestamo)).Returns(expectedResponse);

        var result = await _prestamoService.IngresarPrestamo(crearPrestamoDto);

        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.FechaMaximaDevolucion.Date, result.FechaMaximaDevolucion.Date);
    }

    [Fact]
    public async void IngresarPrestamoError()
    {

        var crearPrestamoDto = new CrearPrestamoDTO
        {
            IdentificacionUsuario = "110872718",
            Isbn = Guid.NewGuid().ToString(),
            TipoUsuario = (int)TipoUsuarioPrestamo.INVITADO
        };


        var prestamo = new Prestamo { Id = Guid.NewGuid(), IdentificacionUsuario = crearPrestamoDto.IdentificacionUsuario };

        _mockMapper.Setup(mapper => mapper.Map<Prestamo>(crearPrestamoDto)).Returns(prestamo);
        _mockPrestamoRepository.Setup(repo => repo.IngresarPrestamo(prestamo))
            .ThrowsAsync(new Exception("Error al ingresar el préstamo"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _prestamoService.IngresarPrestamo(crearPrestamoDto));
        Assert.Equal("Error al ingresar el préstamo", exception.Message);

    }

    [Fact]
    public async Task ConsultartamoExitoso()
    {
        var prestamoId = Guid.NewGuid();
        var prestamo = new Prestamo { Id = prestamoId, IdentificacionUsuario = "110872718", TipoUsuario = (int)TipoUsuarioPrestamo.INVITADO };
        var expectedDto = new ConsultaPrestamoTO { Id = prestamoId, IdentificacionUsuario = "110872718", TipoUsuario = (int)TipoUsuarioPrestamo.INVITADO };

        _mockPrestamoRepository.Setup(repo => repo.Consultar(prestamoId)).ReturnsAsync(prestamo);
        _mockMapper.Setup(mapper => mapper.Map<ConsultaPrestamoTO>(prestamo)).Returns(expectedDto);

        var result = await _prestamoService.Consultar(prestamoId);

        Assert.NotNull(result);
        Assert.Equal(expectedDto.Id, result.Id);
        Assert.Equal(expectedDto.IdentificacionUsuario, result.IdentificacionUsuario);
        Assert.Equal(expectedDto.TipoUsuario, result.TipoUsuario);
    }

    [Fact]
    public async Task ConsultarPrestamoError()
    {
        var prestamoId = Guid.NewGuid();
        _mockPrestamoRepository.Setup(repo => repo.Consultar(prestamoId)).ReturnsAsync((Prestamo)null);
        var result = await _prestamoService.Consultar(prestamoId);
        Assert.Null(result);
    }

    [Fact]
    public async Task ConsultarPrestamoUsuarioInvitadoExitoso()
    {
        string identificacionUsuario = "110872718";
        _mockPrestamoRepository.Setup(repo => repo.ConsultarPrestamoUsuarioInvitado(identificacionUsuario)).ReturnsAsync(true);
        var result = await _prestamoService.ConsultarPrestamoUsuarioInvitado(identificacionUsuario);
        Assert.True(result);
    }

    [Fact]
    public async Task ConsultarPrestamoUsuarioInvitadoError()
    {
        string identificacionUsuario = "110872718";
        _mockPrestamoRepository.Setup(repo => repo.ConsultarPrestamoUsuarioInvitado(identificacionUsuario)).ReturnsAsync(false);
        var result = await _prestamoService.ConsultarPrestamoUsuarioInvitado(identificacionUsuario);
        Assert.False(result);
    }

    [Theory]
    [InlineData(TipoUsuarioPrestamo.AFILIADO, "2024-10-31", "2024-11-14")]
    [InlineData(TipoUsuarioPrestamo.EMPLEADO, "2024-10-31", "2024-11-12")]
    [InlineData(TipoUsuarioPrestamo.INVITADO, "2024-10-31", "2024-11-11")]
    public void CalcularFechaEntregaExito(TipoUsuarioPrestamo tipoUsuario, string fechaInicioStr, string fechaEsperadaStr)
    {
        DateTime fechaInicio = DateTime.Parse(fechaInicioStr);
        DateTime fechaEsperada = DateTime.Parse(fechaEsperadaStr);
        DateTime fechaDevolucion = PrestamoService.CalcularFechaEntrega(tipoUsuario, fechaInicio);

        Assert.Equal(fechaEsperada, fechaDevolucion);
    }


    [Theory]
    [InlineData(TipoUsuarioPrestamo.AFILIADO, 10)]
    [InlineData(TipoUsuarioPrestamo.EMPLEADO, 8)]
    [InlineData(TipoUsuarioPrestamo.INVITADO, 7)]
    public void CalcularFechaEntregaError(TipoUsuarioPrestamo tipoUsuario, int diasEsperados)
    {
        DateTime fechaInicio = new DateTime(2024, 10, 31);
        DateTime fechaEsperadaIncorrecta = fechaInicio.AddDays(diasEsperados);
        DateTime fechaDevolucion = PrestamoService.CalcularFechaEntrega(tipoUsuario, fechaInicio);

        Assert.NotEqual(fechaEsperadaIncorrecta, fechaDevolucion);
    }

}
