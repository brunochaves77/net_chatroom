using ChatRoom.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.WebApplication.Controllers {

    [Route("api")]
    [ApiController]
    public class RoomController : ControllerBase {

        private readonly RoomService _roomService;

        public RoomController(RoomService roomService) {
            _roomService = roomService;
        }
    }
}
