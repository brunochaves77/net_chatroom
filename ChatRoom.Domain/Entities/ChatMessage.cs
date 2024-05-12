﻿namespace ChatRoom.Domain.Entities;

public class ChatMessage
{
    
    public Guid Id { get; set; }

    public required DateTime ReceivedAt { get; set; }
    
    public required string Message { get; set; }
    
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    
    
}