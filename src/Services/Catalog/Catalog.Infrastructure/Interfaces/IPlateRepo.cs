using System.Collections.Generic;
using Catalog.Domain;

namespace Catalog.Infrastructure.Interfaces
{
    public interface IPlateRepo
    {
        Task<IEnumerable<Plate>> GetAll();

        Task Add(Plate plate);
    }
}
