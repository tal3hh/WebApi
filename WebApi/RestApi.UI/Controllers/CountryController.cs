using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs.Country;
using ServiceLayer.Services.Interfaces;
using System.Data;

namespace Api.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryservice;
        private readonly ILogger<CountryController> _logger;
        private readonly IWebHostEnvironment _env;

        public CountryController(ICountryService countryservice, ILogger<CountryController> logger, IWebHostEnvironment env)
        {
            _countryservice = countryservice;
            _logger = logger;
            _env = env;
        }

        [ResponseCache(Duration = 10)] //10saniye RAM'da saxliyir.(Database getmeden, datalari getirir.)
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll index'e istek atilmisdir.");
            return Ok(await _countryservice.GetAllAsync());
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int id)
        {
            _logger.LogInformation("GetbyId index'e istek atilmisdir.");
            return Ok(await _countryservice.GetById(id));
        }

        [Authorize]
        [HttpPost]
        [Route("Create(Authorize)")]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Create index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Create index'e istek atilmisdir.");
            await _countryservice.CreateAsync(dto);

            return Ok("Data created...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CountryUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Update index'e istek atilmisdir. Validation error.(ERROR)");
                return BadRequest();
            }

            _logger.LogInformation("Update index'e istek atilmisdir.");
            await _countryservice.UpdateAsync(id, dto);

            return Ok("Data updated...");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Remove(OnlyAdmin)/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            _logger.LogInformation("Remove index'e istek atilmisdir.");
            await _countryservice.RemoveAsync(id);

            return Ok("Data removed...");
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + "_" + file.Name.ToString();
            var path = Path.Combine(_env.WebRootPath, "img", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Created(string.Empty,file);
        }
    }
}

