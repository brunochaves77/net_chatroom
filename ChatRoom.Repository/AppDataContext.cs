using ChatRoom.Domain.Entities;
using ChatRoom.Repository.EntitiesConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Repository {
    public class AppDataContext : IdentityDbContext {

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<User>(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration<Room>(new RoomEntityConfiguration());
            modelBuilder.ApplyConfiguration<ChatMessage>(new ChatMessageEntityConfiguration());

        }

        public DbSet<User> DBUsers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }



    }
}
