using AutoMapper;
using DomainLayer.Entities;
using RepositoryLayer.Repositories.Interfaces;
using RepositoryLayer.UniteOfWork;
using ServiceLayer.DTOs.City;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CityService : ICityService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<City> _city;
        public CityService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _city = _uow.GetRepository<City>();
        }

        public async Task<List<CityDto>> GetAllAsync()
        {
            var entities = await _city.GetAllAsync();

            return _mapper.Map<List<CityDto>>(entities);
        }

        public async Task<CityDto> GetById(int id)
        {
            var entity = await _city.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            return _mapper.Map<CityDto>(entity);
        }

        public async Task CreateAsync(CityCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = _mapper.Map<City>(dto);

            if (entity is null) throw new NullReferenceException();

            await _city.CreateAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CityUpdateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = await _city.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _mapper.Map(dto, entity);

            _city.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _city.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _city.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
