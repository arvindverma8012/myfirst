using System;
using DatingApp.api.Entities;

namespace DatingApp.api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser AppUser);
       
    }
}
