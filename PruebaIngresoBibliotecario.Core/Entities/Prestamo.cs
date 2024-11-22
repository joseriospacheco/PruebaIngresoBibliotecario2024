using PruebaIngresoBibliotecario.Core.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace PruebaIngresoBibliotecario.Core.Entities;
public class Prestamo
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid Isbn { get; set; }
    [Required]
    [MaxLength(10)]
    public string IdentificacionUsuario { get; set; }
    [Required]
    [EnumDataType(typeof(TipoUsuarioPrestamo))]
    public int TipoUsuario { get; set; }
    [Required]
    public DateTime FechaMaximaDevolucion { get; set; }
}
