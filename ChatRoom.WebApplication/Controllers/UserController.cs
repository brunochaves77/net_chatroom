using ChatRoom.Application.Factories;
using ChatRoom.Application.Models.Responses;
using ChatRoom.Application.Models.Requests;
using ChatRoom.Application.Services;
using ChatRoom.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatRoom.WebApplication.Controllers {
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly UserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserService userService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model) 
        {
            if (ModelState.IsValid) 
            {
                var user = new IdentityUser { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) 
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("User registered and logged in successfully.");
                }
                else 
                {
                    // Handle errors
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded) 
                {
                    return Ok("User logged in successfully.");
                }
                else 
                {
                    // Handle login failure
                    return BadRequest("Login failed.");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out successfully.");
        }
    }
}
