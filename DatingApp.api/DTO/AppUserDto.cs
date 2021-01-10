using System;

namespace DatingApp.api.DTO
{
    public class AppUserDto
    {
        public string username { get; set; }
        public string Token { get; set; }
        public string photourl { get; set; }

        public string KnownAs {get;set;}
    }
}
