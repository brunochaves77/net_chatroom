using ChatRoom.Application.Factories;
using ChatRoom.Application.Models.Responses;
using ChatRoom.Application.Services;
using ChatRoom.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.WebApplication.Controllers {
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly UserService _userService;

        public UserController(UserService userService) {
            _userService = userService;
        }

    }
}
