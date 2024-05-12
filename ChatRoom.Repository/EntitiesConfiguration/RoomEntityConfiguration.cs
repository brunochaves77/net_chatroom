using ChatRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoom.Repository.EntitiesConfiguration {
    public class RoomEntityConfiguration : IEntityTypeConfiguration<Room> {

        public void Configure(EntityTypeBuilder<Room> builder) {

            builder.ToTable("ent_room");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

        }


    }
}
