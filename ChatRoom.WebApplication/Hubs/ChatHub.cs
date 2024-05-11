using Microsoft.AspNetCore.SignalR;

namespace ChatRoom.WebApplication.Hubs {
    public class ChatHub : Hub {
        private static Dictionary<string, List<string>> chatRooms = new Dictionary<string, List<string>>();

        public async Task JoinChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;
            if (!chatRooms.ContainsKey(roomName)) {
                chatRooms.Add(roomName, new List<string>());
            }
            chatRooms[roomName].Add(connectionId);
            await Groups.AddToGroupAsync(connectionId, roomName);
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", "BOT", $"VOCÊ ENTROU NO CHAT {roomName}");
        }

        public async Task SendMessage(string roomName, string user, string message) {

            var commandResult = "";

            if (chatRooms.ContainsKey(roomName)) {
                foreach (var connectionId in chatRooms[roomName]) {

                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);

                    if (commandResult != "") {
                        await Clients.Client(connectionId).SendAsync("ReceiveMessage", "BOT", commandResult);
                    }


                }
            }
        }

        public async Task LeaveChatRoom(string roomName) {
            string connectionId = Context.ConnectionId;
            if (chatRooms.ContainsKey(roomName) && chatRooms[roomName].Contains(connectionId)) {
                chatRooms[roomName].Remove(connectionId);
                await Groups.RemoveFromGroupAsync(connectionId, roomName);
            }
        }
    }
}
