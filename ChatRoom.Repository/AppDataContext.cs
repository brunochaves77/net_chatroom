using ChatRoom.Domain.Entities;
using ChatRoom.Repository.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Repository {
    public class AppDataContext : DbContext {

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<User>(new UserEntityConfiguration());

        }

        public DbSet<User> Users { get; set; }


    }
}
