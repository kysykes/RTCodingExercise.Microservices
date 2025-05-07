using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Services;
using Catalog.Domain;
using Catalog.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class PlateServiceTests
    {
        private readonly Mock<IPlateRepo> _plateRepoMock;
        private readonly IMapper _mapper;
        private readonly PlateService _plateService;

        public PlateServiceTests()
        {
            _plateRepoMock = new Mock<IPlateRepo>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Plate, PlateDto>();
            });

            _mapper = config.CreateMapper();

            _plateService = new PlateService(_plateRepoMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPlatesAsync_ReturnsMappedPlateDtos()
        {
            // Arrange
            var plateList = new List<Plate>
        {
            new Plate
            {
                Id = Guid.NewGuid(),
                Registration = "ABC123",
                PurchasePrice = 1000,
                SalePrice = 1500,
                Letters = "ABC",
                Numbers = 123
            },
            new Plate
            {
                Id = Guid.NewGuid(),
                Registration = "XYZ789",
                PurchasePrice = 2000,
                SalePrice = 2500,
                Letters = "XYZ",
                Numbers = 789
            }
        };

            _plateRepoMock.Setup(repo => repo.GetAll())
                          .ReturnsAsync(plateList);

            // Act
            var result = await _plateService.GetAllPlates();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            var resultList = result.ToList();

            Assert.Equal("ABC123", resultList[0].Registration);
            Assert.Equal(1000, resultList[0].PurchasePrice);
            Assert.Equal("XYZ789", resultList[1].Registration);
            Assert.Equal(789, resultList[1].Numbers);
        }

        [Fact]
        public async Task GetAllPlates_ThrowsException_PropagatesToCaller()
        {
            // Arrange
            _plateRepoMock.Setup(repo => repo.GetAll())
                          .ThrowsAsync(new Exception("Database failure"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _plateService.GetAllPlates());
            Assert.Equal("Database failure", ex.Message);
        }

        [Fact]
        public async Task GetAllPlates_WhenNoPlatesExist_ReturnsEmptyList()
        {
            // Arrange
            _plateRepoMock.Setup(repo => repo.GetAll())
                          .ReturnsAsync(new List<Plate>());

            // Act
            var result = await _plateService.GetAllPlates();

            // Assert
            Assert.NotNull(result); // Should never return null
            Assert.Empty(result);   // List should be empty
        }

    }
}