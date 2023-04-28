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
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IUserService userservice, ILogger<EmployeeController> logger)
        {
            _userservice = userservice;
            _logger = logger;
        }

        [ResponseCache(Duration = 10)] //10saniye RAM'da saxliyir.(Database getmeden, datalari getirir.)
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll index'e istek atilmisdir.");
            return Ok(await _userservice.GetAllAsync());
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int id)
        {
            _logger.LogInformation("GetbyId index'e istek atilmisdir.");
            return Ok(await _userservice.GetById(id));
        }

        [HttpGet]
        [Route("SearchName/{name}")]
        public async Task<IActionResult> SearchName(string name)
        {
            _logger.LogInformation("SearchName index'e istek atilmisdir.");
            return Ok(await _userservice.GetAllByName(name));
        }

        [HttpGet]
        [Route("SearchSurname/{surname}")]
        public async Task<IActionResult> SearchSurname(string surname)
        {
            _logger.LogInformation("SearchSurname index'e istek atilmisdir.");
            return Ok(await _userservice.GetAllBySurname(surname));
        }

        [HttpGet]
        [Route("SearchAdress/{adress}")]
        public async Task<IActionResult> SearchAdress(string adress)
        {
            _logger.LogInformation("SearchAdress index'e istek atilmisdir.");
            return Ok(await _userservice.GetAllByAdress(adress));
        }

        [HttpGet]
        [Route("SearchAge")]
        public async Task<IActionResult> SearchAge(int min, int max)
        {
            _logger.LogInformation("SearchAge index'e istek atilmisdir.");
            return Ok(await _userservice.GetAllByAge(min, max));
        }

        [Authorize]
        [HttpPost]
        [Route("Create(Authorize)")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Create index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Create index'e istek atilmisdir.");
            await _userservice.CreateAsync(dto);

            return Ok("Data created...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Update index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Update index'e istek atilmisdir.");
            await _userservice.UpdateAsync(id, dto);

            return Ok("Data updated...");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Remove(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            _logger.LogInformation("Remove index'e istek atilmisdir.");
            await _userservice.RemoveAsync(id);

            return Ok("Data removed...");
        }
    }
}
