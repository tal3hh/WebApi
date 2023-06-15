using ServiceLayer.DTOs.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<CountryDto>> GetAllAsync();
        Task<CountryDto> GetById(int id);
        Task CreateAsync(CountryCreateDto dto);
        Task UpdateAsync(int id, CountryUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
