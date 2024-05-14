using ChatRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoom.Repository.EntitiesConfiguration {
    public class UserEntityConfiguration : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {

            builder.ToTable("ent_user");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName).HasMaxLength(32).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(32).IsRequired();

        }
    }
}
