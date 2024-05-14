using ChatRoom.Domain.Entities;

namespace ChatRoom.Application.Models;

public class UserChatData
{
    public required Room Room { get; set; }
    public required string SignalConnectionId { get; set; }
}