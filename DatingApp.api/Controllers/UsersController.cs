using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatingApp.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DatingApp.api.Interfaces;
using DatingApp.api.Entities;
using DatingApp.api.DTO;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DatingApp.api.Extensions;

namespace DatingApp.API.Controllers
{
    [Authorize]
   [ApiController]
    [Route("api/[controller]")]
       public class UsersController : ControllerBase
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
        // GET api/values
        
       
      [HttpGet("users")]
      //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users =(await _userRepository.GetMembersAsync());

            //var usersToReturn=_mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(users);
        }

        // GET api/values/5
        //[Authorize]
        [HttpGet("{username}",Name="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return  await _userRepository.GetMemberAsync(username);
           
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
           
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

         [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
           
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var  result=await _photoService.AddPhotoAsync(file);
            if(result.Error!=null) return BadRequest(result.Error.Message);

            var photo=new Photo
            {
               Url=result.SecureUrl.AbsoluteUri,
               PublicId=result.PublicId
            };
              if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync())
            {
         
               return CreatedAtRoute("GetUser",new{username=user.username},_mapper.Map<PhotoDto>(photo));
            } 
              return BadRequest("Problem adding photo ");
            }




        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

      [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    


    }

        

    
}

