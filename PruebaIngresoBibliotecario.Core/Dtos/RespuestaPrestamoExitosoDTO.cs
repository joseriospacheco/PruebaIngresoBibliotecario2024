namespace PruebaIngresoBibliotecario.Core.Dtos;
public record RespuestaPrestamoExitosoDTO
{
    public Guid Id { get; init; }
    public DateTime FechaMaximaDevolucion { get; init; }
}
