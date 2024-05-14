using ChatRoom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoom.Repository.EntitiesConfiguration {
    public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessage> {

        public void Configure(EntityTypeBuilder<ChatMessage> builder) {

            builder.ToTable("ent_chat_message");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.RoomId, x.ReceivedAt });

            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.RoomId).IsRequired();

            builder.Property(x => x.Message).HasMaxLength(280).IsRequired();
            builder.Property(x => x.ReceivedAt).IsRequired();

            builder.HasOne(x => x.Room).WithMany().HasForeignKey(x => x.RoomId);

        }


    }
}
