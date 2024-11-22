namespace PruebaIngresoBibliotecario.Core.Dtos;
public record ConsultaPrestamoTO
{
    public Guid Id { get; init; }
    public Guid Isbn { get; init; }
    public string IdentificacionUsuario { get; init; }
    public int TipoUsuario { get; init; }
    public DateTime FechaMaximaDevolucion { get; init; }
}
