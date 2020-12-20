
using DatingApp.api.Models;
using DatingApp.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options ):base(options) 
         { }
        public DbSet<Value> Values { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
    }
}