using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;

namespace ChatRoom.Repository.Repositories {
    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IRepositoryBase<ChatMessage> {

        public ChatMessageRepository(AppDataContext appDataContext) : base(appDataContext) { }

        public IEnumerable<ChatMessage> GetLatestMessagesFromRoom(Guid roomId, int registerCount = 50) {

            var result = Context.ChatMessages
                .Where(x => x.RoomId == roomId)
                .OrderByDescending(x => x.ReceivedAt)
                .Take(registerCount);

            return result;

        }

    }
}
