using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.api.DTO;
using DatingApp.api.Entities;

namespace DatingApp.api.Interfaces
{
    public interface IUserRepository
    {
       void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<bool> SaveAllAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);
        //Task<string> GetUserGender(string username); 
    }
}
