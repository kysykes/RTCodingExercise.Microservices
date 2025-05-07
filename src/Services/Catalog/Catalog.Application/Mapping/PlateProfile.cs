using AutoMapper;
using Catalog.Domain;
using Catalog.Application.Dtos;

namespace Catalog.Application.Mapping
{
    public class PlateProfile : Profile
    {
        public PlateProfile()
        {
            CreateMap<Plate, PlateDto>();
        }
    }
}
