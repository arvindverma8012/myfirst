using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatingApp.api.DTO
{
    public class LoginDto
    {
         [Required]
       
        public string username { get; set; }

         [Required]
         
        public string password { get; set; }
    }
}
