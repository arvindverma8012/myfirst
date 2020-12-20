using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.api.DTO;
using DatingApp.api.Entities;
using DatingApp.api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
            _context=context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
             return await _context.AppUser
                .Where(x => x.username == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();               
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
           return await _context.AppUser.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();   
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.AppUser.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
          return await _context.AppUser.Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.username==username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
           return await _context.AppUser.Include(p=>p.Photos).ToListAsync(); 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State=EntityState.Modified;
        }
    }
}
