using System;
using DatingApp.api;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using DatingApp.api.Data;
using System.Text;
using DatingApp.api.Entities;
using DatingApp.api.DTO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.api.Interfaces;
using System.Linq;

namespace DatingApp.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AccountController:ControllerBase

    {
        private readonly DataContext _context;

        public ITokenService _tokenService { get; }

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context=context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto registerDto )
        {
          if(await UserExists(registerDto.username)) return BadRequest("UserName is taken");
          using var hmac =new HMACSHA512();
          var user =new AppUser
          {
              username=registerDto.username.ToLower(),
              PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
              PasswordSalt=hmac.Key
          };
          _context.Add(user);
          await _context.SaveChangesAsync();
          
          return new AppUserDto
          {
            username=user.username,
            Token=_tokenService.CreateToken(user),


          };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.AppUser.AnyAsync(x =>x.username==username.ToLower());
        }



        
    [HttpPost("Login")]
    public async Task<ActionResult<AppUserDto>> Login(LoginDto loginDto)
    {
        var user=await _context.AppUser.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.username==loginDto.username);
        if(user==null) return Unauthorized("Invalid Username");
        using var hmac=new HMACSHA512(user.PasswordSalt);
        var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
        for (int i=0;i<computedHash.Length;i++)
        {
            if(computedHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
           return new AppUserDto
          {
            username=user.username,
            Token=_tokenService.CreateToken(user),
            photourl=user.Photos.FirstOrDefault(x =>x.IsMain)?.Url

          };
    }

    }

}
