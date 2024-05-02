using GameLibrary.Core.Users;
using GameService.Entity.DBEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameService.Entity.DBEntity
{




    public class MyDbContext : DbContext
    {
        static int count = 0;
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            ++count;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>();
            base.OnModelCreating(modelBuilder);
        }
    }


    

}
