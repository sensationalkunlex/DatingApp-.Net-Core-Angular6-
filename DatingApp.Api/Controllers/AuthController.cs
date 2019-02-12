using System.Threading.Tasks;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController :ControllerBase
    {
        private readonly IAuthRepository _repo;
          private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo , IConfiguration config)
        {
            _repo = repo;
            _config=config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            userDto.Username=userDto.Username.ToLower();
            if(await _repo.UserExists( userDto.Username))
            return BadRequest("Username already exists");
            var userToCreate=new User{
                Username= userDto.Username

            };
            var createdUser= await _repo.Register(userToCreate,  userDto.Password);
            return StatusCode(201);
        }

           [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            var userFromRepo= await _repo.Login(userDto.Username.ToLower(), userDto.Password);
            if(userFromRepo==null)
            return Unauthorized();
            var claims= new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            var Key= new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds=new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor= new  SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddHours(7),
                SigningCredentials=creds
            };
            var tokenHander= new JwtSecurityTokenHandler();
            var token =tokenHander.CreateToken(tokenDescriptor);
            return Ok(new {
                token=tokenHander.WriteToken(token)
            });

             


        }
    }
} 