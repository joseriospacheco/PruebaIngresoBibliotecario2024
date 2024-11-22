using AutoMapper;
using PruebaIngresoBibliotecario.Core.Dtos;
using PruebaIngresoBibliotecario.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Infrastructure.Mappings;
public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<CrearPrestamoDTO, Prestamo>();

        //OJO
        CreateMap<Prestamo, ConsultaPrestamoTO>()
        .ForMember(vm => vm.FechaMaximaDevolucion, m => m.MapFrom(u => (u.FechaMaximaDevolucion.ToShortDateString())));

        CreateMap<Prestamo, RespuestaPrestamoExitosoDTO>();
    }
}
