using ChatRoom.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.WebApplication.Controllers {

    [Route("api")]
    [ApiController]
    public class ChatMessageController : ControllerBase {

        private readonly ChatMessageService _chatMessageService;

        public ChatMessageController(ChatMessageService chatMessageService) { 
            _chatMessageService = chatMessageService;
        }
        
        [HttpGet("messages/latest-by-room-id/{roomId}")]
        public IActionResult GetChatMessages(Guid roomId) 
        {
            var chatMessages = _chatMessageService.GetLatestMessagesFromRoom(roomId);
            return Ok(chatMessages);
        }

    }
    
    
}
