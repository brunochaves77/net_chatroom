using ChatRoom.Application.Models;
using ChatRoom.Domain.Entities;
using ChatRoom.Repository.Repositories;
using ChatRoom.WebApplication.ApplicationService;
using ChatRoom.WebApplication.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoom.WebApplication.Hubs {

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatHub : Hub {
        private static Dictionary<string, UserChatData> _chatRooms = new Dictionary<string, UserChatData>();
        
        private readonly ChatMessageRepository _chatMessageRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoomRepository _roomRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        

        public ChatHub(ChatMessageRepository chatMessageRepository, UserManager<IdentityUser> userManager, RoomRepository roomRepository, IHttpContextAccessor httpContextAccessor)
        {
            _roomRepository = roomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }


        

        

        public async Task JoinChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;

            var room = _roomRepository.GetFirstOrDefault(x => x.Name == roomName);
            var httpContext = _httpContextAccessor.HttpContext;

            if (room == null)
            {
                room = new Room() {Name = roomName, Id = Guid.NewGuid()};
                _roomRepository.Add(room);
                _roomRepository.Commit();
            }

            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);

            if (user == null) 
                throw new Exception("Invalid session.");

            _chatRooms.Remove(user.UserName);
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
            
            _chatRooms.Add(user.UserName, new UserChatData(){Room = room, SignalConnectionId = connectionId});
            
            await Groups.AddToGroupAsync(connectionId, roomName);
            await Clients.Client(connectionId).SendAsync("GetMessages", room.Id);
        }

        public async Task SendMessage(string message) {
            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            
            if (user == null) 
                throw new Exception("Invalid session.");

            var userChatData = _chatRooms[user.UserName];
            
            if (userChatData == null)
                throw new Exception("User is not assigned to a room.");
            
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(), 
                ReceivedAt = DateTime.UtcNow, 
                Username = user.UserName, 
                Message = message, 
                RoomId = userChatData.Room.Id,
            };
            
            if (!chatMessage.Message.StartsWith("/stock"))
            {
                _chatMessageRepository.Add(chatMessage);
                _chatMessageRepository.Commit();
            }

            foreach (var userKey in _chatRooms)
            {
               if (userKey.Value.Room.Id != userChatData.Room.Id)
                   continue;
               
               await Clients.Client(userKey.Value.SignalConnectionId).SendAsync("ReceiveMessage", user, message);
            }

            RabbitMQApplicationService.SendMessageFromRabbitMQ(chatMessage.Username, userChatData.Room.Name, chatMessage.Message);

        }

        public async Task LeaveChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;
            
            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.GetUserAsync(Context.User);
            
            if (user == null) 
                throw new Exception("Invalid session.");
            
            _chatRooms.Remove(user.UserName);
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
        }
    }
}
