using System;
using System.ComponentModel.DataAnnotations;
namespace DatingApp.api.DTO
{
    public class RegisterDto
    {   
         [Required]
        public string username { get; set; }

         [Required]
        [StringLength(8,MinimumLength=4)]
        public string password { get; set; }
    }
}
