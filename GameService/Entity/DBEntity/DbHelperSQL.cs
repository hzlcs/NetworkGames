using GameLibrary.Core.Users;
using GameService.Entity.DBEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameService.Entity.DBEntity
{




    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();
            base.OnModelCreating(modelBuilder);
        }
    }


    

}
