using AutoMapper;
using DomainLayer.Entities;
using RepositoryLayer.Repositories.Interfaces;
using RepositoryLayer.UniteOfWork;
using ServiceLayer.DTOs.Country;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<Country> _country;
        public CountryService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _country = _uow.GetRepository<Country>();
        }

        public async Task<List<CountryDto>> GetAllAsync()
        {
            var entities = await _country.GetAllAsync();

            return _mapper.Map<List<CountryDto>>(entities);
        }

        public async Task<CountryDto> GetById(int id)
        {
            var entity = await _country.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            return _mapper.Map<CountryDto>(entity);
        }

        public async Task CreateAsync(CountryCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = _mapper.Map<Country>(dto);

            if (entity is null) throw new NullReferenceException();

            await _country.CreateAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CountryUpdateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = await _country.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _mapper.Map(dto, entity);

            _country.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _country.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _country.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
