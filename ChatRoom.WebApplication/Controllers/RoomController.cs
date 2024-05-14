using ChatRoom.Application.Services;
using ChatRoom.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.WebApplication.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase 
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService) 
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IActionResult GetAllRooms() 
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoom(Guid id) 
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) 
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRoom([FromBody] Room room) 
        {
            _roomService.CreateRoom(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteRoom(Guid id) 
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) 
            {
                return NotFound();
            }

            _roomService.DeleteRoom(room);
            return NoContent();
        }
    }
}
