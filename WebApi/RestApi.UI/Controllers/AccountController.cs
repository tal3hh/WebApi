using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTOs.Account;
using ServiceLayer.Services.Interfaces;
using System.Security.Cryptography;

namespace Api.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return NotFound();

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            string token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new AppUser()
            {
                Fullname = dto.Fullname,
                Email = dto.Email,
                UserName = dto.Fullname.Substring(0, 5) + "01",
            };
            await _userManager.CreateAsync(user, dto.Password);
            await _userManager.AddToRoleAsync(user, "Member");

            return Ok("User Created...");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("CreateRole(OnlyAdmin)")]
        public async Task<IActionResult> CreateRole([FromQuery] string role)
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = role
            });

            return Ok("Created role...");
        }


    }
}
