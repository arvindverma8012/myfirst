using System;
using System.Linq;
using AutoMapper;
using DatingApp.api.DTO;
using DatingApp.api.Entities;
using DatingApp.api.Extensions;

namespace DatingApp.api.Helpers
{
    public class AutoMapperProfiles:Profile
    {
       public AutoMapperProfiles()
       {
            CreateMap<AppUser,MemberDto>().
            ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotoDto>();
            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<RegisterDto,AppUser>();
       } 
    }
}
