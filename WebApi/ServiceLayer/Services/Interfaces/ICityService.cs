using ServiceLayer.DTOs.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface ICityService
    {
        Task<List<CityDto>> GetAllAsync();
        Task<CityDto> GetById(int id);
        Task CreateAsync(CityCreateDto dto);
        Task UpdateAsync(int id, CityUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
