using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain;
using Catalog.Infrastructure.Interfaces;

namespace Catalog.Application.Services
{
    public class PlateService : IPlateService
    {
        private readonly IPlateRepo _plateRepo;
        private readonly IMapper _mapper;

        public PlateService(IPlateRepo plateRepository, IMapper mapper)
        {
            _plateRepo = plateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlateDto>> GetAllPlates()
        {
            var plates =  await _plateRepo.GetAll();
            return _mapper.Map<IEnumerable<PlateDto>>(plates);
        }

        // Adds a new plate and returns the added plate
        public async Task<PlateDto> AddPlate(PlateDto plateDto)
        {
            // Convert PlateDto to Plate entity (model)
            var plate = new Plate
            {
                Id = plateDto.Id,
                Registration = plateDto.Registration,
                PurchasePrice = plateDto.PurchasePrice,
                SalePrice = plateDto.SalePrice,
                Letters = plateDto.Letters,
                Numbers = plateDto.Numbers
            };

            // Save the new plate in the database
            await _plateRepo.Add(plate);

            // Return the saved plate as PlateDto
            return new PlateDto
            {
                Id = plate.Id,
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice,
                Letters = plate.Letters,
                Numbers = plate.Numbers
            };
        }
    }
}

