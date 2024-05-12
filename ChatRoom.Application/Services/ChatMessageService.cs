using ChatRoom.Domain.Entities;
using ChatRoom.Domain.Interfaces;
using ChatRoom.Repository.Repositories;

namespace ChatRoom.Application.Services {
    public class ChatMessageService : ServiceBase<ChatMessage>, IServiceBase<ChatMessage> {

        private readonly ChatMessageRepository _chatMessageRepository;

        public ChatMessageService(ChatMessageRepository chatMessageRepository) : base(chatMessageRepository) {
            _chatMessageRepository = chatMessageRepository;
        }

    }
}
