using ServiceLayer.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task CreateAsync(UserCreateDto dto);
        Task UpdateAsync(int id, UserUpdateDto dto);
        Task<List<UserDto>> GetAllByAge(int min, int max);
        Task<UserDto> GetById(int id);
        Task RemoveAsync(int id);
        Task<List<UserDto>> GetAllByName(string name);
        Task<List<UserDto>> GetAllByAdress(string adress);
        Task<List<UserDto>> GetAllBySurname(string surname);
    }
}
