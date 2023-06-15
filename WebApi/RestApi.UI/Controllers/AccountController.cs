using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs.Account;
using ServiceLayer.Services.Interfaces;
using System.Security.Cryptography;
using System;

namespace Api.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Login index'e istek atilmisdir.(BadRequest)");
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                _logger.LogWarning("Login index'e istek atilmisdir. User is null.(NotFound)");
                return NotFound();
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                _logger.LogWarning("Login index'e istek atilmisdir.(Unauthorized)");
                return Unauthorized();
            }
            
            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);
            _logger.LogInformation("Login index'e istek atilmisdir.");

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Register index'e istek atilmisdir.Validation error.(BadRequest)");
                return BadRequest();
            }

            var user = new AppUser()
            {
                Fullname = dto.Fullname,
                Email = dto.Email,
                UserName = string.Concat(dto.Fullname.AsSpan(0, 5), "01"),
            };

            await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            return Ok("User Created...");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CreateRole(OnlyAdmin)")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            if(role is null)
            {
                _logger.LogError("CreateRole index'e istek atilmisdir. Validation error. (BadRequest)");
                return BadRequest();
            }

            _logger.LogInformation("CreateRole index'e istek atilmisdir.");
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = role
            });

            return Ok("Created role...");
        }


    }
}
