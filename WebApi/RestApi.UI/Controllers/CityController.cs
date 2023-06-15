using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs.City;
using ServiceLayer.Services.Interfaces;
using System.Data;

namespace Api.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _Cityservice;
        private readonly ILogger<CityController> _logger;

        public CityController(ICityService Cityservice, ILogger<CityController> logger)
        {
            _Cityservice = Cityservice;
            _logger = logger;
        }

        [ResponseCache(Duration = 10)] //10saniye RAM'da saxliyir.(Database getmeden, datalari getirir.)
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll index'e istek atilmisdir.");
            return Ok(await _Cityservice.GetAllAsync());
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int id)
        {
            _logger.LogInformation("GetbyId index'e istek atilmisdir.");
            return Ok(await _Cityservice.GetById(id));
        }

        [Authorize]
        [HttpPost]
        [Route("Create(Authorize)")]
        public async Task<IActionResult> Create([FromBody] CityCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Create index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Create index'e istek atilmisdir.");
            await _Cityservice.CreateAsync(dto);

            return Ok("Data created...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CityUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Update index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Update index'e istek atilmisdir.");
            await _Cityservice.UpdateAsync(id, dto);

            return Ok("Data updated...");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Remove(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            _logger.LogInformation("Remove index'e istek atilmisdir.");
            await _Cityservice.RemoveAsync(id);

            return Ok("Data removed...");
        }
    }
}
