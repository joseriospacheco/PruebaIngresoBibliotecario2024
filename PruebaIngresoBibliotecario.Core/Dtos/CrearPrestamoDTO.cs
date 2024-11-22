namespace PruebaIngresoBibliotecario.Core.Dtos;
public record CrearPrestamoDTO
{
    public string Isbn { get; init; }
    public string IdentificacionUsuario { get; init; }
    public int TipoUsuario { get; init; }
}
