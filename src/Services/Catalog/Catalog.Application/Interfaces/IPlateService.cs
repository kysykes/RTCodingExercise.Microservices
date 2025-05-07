using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface IPlateService
    {
        Task<IEnumerable<PlateDto>> GetAllPlates();

        Task<PlateDto> AddPlate(PlateDto plateDto);
    }
}
