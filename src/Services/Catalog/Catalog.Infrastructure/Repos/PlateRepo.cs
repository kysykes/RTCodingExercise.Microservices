using System.Collections.Generic;
using System.Linq;
using Catalog.Domain;
using Catalog.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repos
{
    public class PlateRepo : IPlateRepo
    {
        private readonly CatalogDbContext _context;

        public PlateRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plate>> GetAll()
        {
            return await _context.Set<Plate>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Add(Plate plate)
        {
            await _context.Plates.AddAsync(plate);
            await _context.SaveChangesAsync();
        }
    }
}
