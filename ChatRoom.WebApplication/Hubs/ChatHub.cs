using ChatRoom.Application.Models;
using ChatRoom.Domain.Entities;
using ChatRoom.Repository.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoom.WebApplication.Hubs {
    public class ChatHub : Hub {
        private static Dictionary<IdentityUser, UserChatData> _chatRooms = new Dictionary<IdentityUser, UserChatData>();
        
        private readonly ChatMessageRepository _chatMessageRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoomRepository _roomRepository;

        public ChatHub(ChatMessageRepository chatMessageRepository, UserManager<IdentityUser> userManager, RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            _chatMessageRepository = chatMessageRepository;
            _userManager = userManager;
        }

        public async Task JoinChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;
            var room = _roomRepository.GetFirstOrDefault(x => x.Name == roomName);
            if (room == null)
            {
                room = new Room() {Name = roomName, Id = Guid.NewGuid()};
                _roomRepository.Add(room);
                _roomRepository.Commit();
            }

            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.GetUserAsync(Context.User);
            
            if (user == null) 
                throw new Exception("Invalid session.");

            _chatRooms.Remove(user);
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
            
            _chatRooms.Add(user, new UserChatData(){Room = room, SignalConnectionId = connectionId});
            
            await Groups.AddToGroupAsync(connectionId, roomName);
        }

        public async Task SendMessage(string message) {
            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.GetUserAsync(Context.User);
            
            if (user == null) 
                throw new Exception("Invalid session.");

            var userChatData = _chatRooms[user];
            
            if (userChatData == null)
                throw new Exception("User is not assigned to a room.");

            foreach (var userKey in _chatRooms)
            {
               if (userKey.Value.Room.Id != userChatData.Room.Id)
                   continue;
               
               await Clients.Client(userChatData.SignalConnectionId).SendAsync("ReceiveMessage", user, message);
            }
        }

        public async Task LeaveChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;
            
            if (Context.User == null)
                throw new Exception("Invalid session.");
            
            var user = await _userManager.GetUserAsync(Context.User);
            
            if (user == null) 
                throw new Exception("Invalid session.");
            
            _chatRooms.Remove(user);
            await Groups.RemoveFromGroupAsync(connectionId, roomName);
        }
    }
}
