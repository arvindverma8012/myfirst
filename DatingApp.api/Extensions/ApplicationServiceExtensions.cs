using System;
using AutoMapper;
using DatingApp.api.Data;
using DatingApp.api.Helpers;
using DatingApp.api.Interfaces;
using DatingApp.api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
             services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
             services.AddScoped<ITokenService,TokenService>();
             services.AddScoped<IPhotoService,PhotoService>();
             services.AddScoped<IUserRepository,UserRepository>();
             services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(x =>x.UseSqlite
            (config.GetConnectionString("DefaultConnection")) ); 
            return services;
        }
    }
}
