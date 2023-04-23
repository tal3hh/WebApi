using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs.User;
using ServiceLayer.Services.Interfaces;
using System.Data;

namespace Api.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userservice;

        public EmployeeController(IUserService userservice)
        {
            _userservice = userservice;
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userservice.GetAllAsync());
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int id)
        {
            return Ok(await _userservice.GetById(id));
        }

        [HttpGet]
        [Route("SearchName/{name}")]
        public async Task<IActionResult> SearchName(string name)
        {
            return Ok(await _userservice.GetAllByName(name));
        }

        [HttpGet]
        [Route("SearchSurname/{surname}")]
        public async Task<IActionResult> SearchSurname(string surname)
        {
            return Ok(await _userservice.GetAllBySurname(surname));
        }

        [HttpGet]
        [Route("SearchAdress/{adress}")]
        public async Task<IActionResult> SearchAdress(string adress)
        {
            return Ok(await _userservice.GetAllByAdress(adress));
        }

        [HttpGet]
        [Route("SearchAge")]
        public async Task<IActionResult> SearchAdress(int min, int max)
        {
            return Ok(await _userservice.GetAllByAge(min, max));
        }

        [Authorize]
        [HttpPost]
        [Route("Create(Authorize)")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _userservice.CreateAsync(dto);

            return Ok("Data created...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _userservice.UpdateAsync(id, dto);

            return Ok("Data updated...");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Remove(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _userservice.RemoveAsync(id);

            return Ok("Data removed...");
        }
    }
}
