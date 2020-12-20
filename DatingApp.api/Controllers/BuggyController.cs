using System;
using DatingApp.api.Data;
using DatingApp.api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.api.Controllers
{
     [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        public readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
           var thing=_context.AppUser.Find(-1);
           if(thing==null) return NotFound();
           return Ok(thing);
        }


        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing=_context.AppUser.Find(-1);
            var thingToReturn=thing.ToString();
            return thingToReturn;
        }
       
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This is not a good request");
        }


    }
}
