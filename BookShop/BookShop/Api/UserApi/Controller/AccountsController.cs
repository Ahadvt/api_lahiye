using AutoMapper;
using BookShop.Api.UserApi.Dtos;
using BookShop.Data.Dal;
using BookShop.Data.Entetiy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        //[HttpGet("createrole")]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("User"));
        //    return StatusCode(201);
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDtos register)
        {
            AppUser user = await _userManager.FindByNameAsync(register.UserName);
            if (user != null) return BadRequest();
             user = new AppUser
            {
                UserName=register.UserName,
                FullName=register.FullName
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var rolemanager = await _userManager.AddToRoleAsync(user, "User");

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDtos login)
        {
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null) return NotFound();
            if (!await _userManager.CheckPasswordAsync(user,login.Password)) return NotFound();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("FullName",user.FullName)
            };
            var role = await _userManager.GetRolesAsync(user);
            claims.AddRange(role.Select(x => new Claim(ClaimTypes.Role, x)).ToList());
            string keystr = "5adf5d5f-f549-4f03-84fe-08c6125561c4";
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keystr));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken
                (
                   claims: claims,
                   signingCredentials: creds,
                   expires: DateTime.Now.AddDays(3),
                   issuer: "https://localhost:44386/",
                   audience: "https://localhost:44386/"

                );
            string tokenstr = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenstr });

            


        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserGtetDto userGtet = _mapper.Map<UserGtetDto>(user);
            return Ok(userGtet);
        }
    }
}
