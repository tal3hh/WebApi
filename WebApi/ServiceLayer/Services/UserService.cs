using AutoMapper;
using DomainLayer.Entities;
using RepositoryLayer.Repositories.Interfaces;
using RepositoryLayer.UniteOfWork;
using ServiceLayer.DTOs.User;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{

    public class UserService : IUserService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _user;
        public UserService(IUow uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _user = _uow.GetRepository<User>();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var entities = await _user.GetAllAsync();

            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<UserDto> GetById(int id)
        {
            var entity = await _user.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<List<UserDto>> GetAllByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();

            var entitylist = await _user.FilterAsync(x => x.Name.Contains(name));

            var dtos = new List<UserDto>();

            foreach (var entity in entitylist)
            {
                dtos.Add(_mapper.Map<UserDto>(entity));
            }

            return dtos;
        }

        public async Task<List<UserDto>> GetAllBySurname(string surname)
        {
            if (string.IsNullOrEmpty(surname)) throw new ArgumentNullException();

            var entitylist = await _user.FilterAsync(x => x.Surname.Contains(surname));

            var dtos = new List<UserDto>();

            foreach (var entity in entitylist)
            {
                dtos.Add(_mapper.Map<UserDto>(entity));
            }

            return dtos;
        }

        public async Task<List<UserDto>> GetAllByAdress(string adress)
        {
            if (string.IsNullOrEmpty(adress)) throw new ArgumentNullException();

            var entitylist = await _user.FilterAsync(x => x.Adress.Contains(adress));

            var dtos = new List<UserDto>();

            foreach (var entity in entitylist)
            {
                dtos.Add(_mapper.Map<UserDto>(entity));
            }

            return dtos;
        }

        public async Task<List<UserDto>> GetAllByAge(int min, int max)
        {
            var entitylist = new List<User>();

            if (min > max)
            {
                entitylist = await _user.FilterAsync(x => x.Age >= min);
            }
            else
            {
                entitylist = await _user.FilterAsync(x => x.Age >= min && x.Age <= max);
            }

            var dtos = new List<UserDto>();

            foreach (var entity in entitylist)
            {
                dtos.Add(_mapper.Map<UserDto>(entity));
            }

            return dtos;
        }

        public async Task CreateAsync(UserCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = _mapper.Map<User>(dto);

            if (entity is null) throw new NullReferenceException();

            await _user.CreateAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UserUpdateDto dto)
        {
            if (dto is null) throw new ArgumentNullException();

            var entity = await _user.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _mapper.Map(dto, entity);

            _user.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _user.FindAsync(id);

            if (entity is null) throw new NullReferenceException();

            _user.Delete(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
