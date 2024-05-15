using ChatRoom.Application.Models.Requests;
using ChatRoom.Application.Services;
using ChatRoom.WebApplication.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.WebApplication.Controllers {
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IdentityService _identityService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(IdentityService identityService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _identityService = identityService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model) {
            if (ModelState.IsValid) {
                var user = new IdentityUser { UserName = model.Username };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("User registered and logged in successfully.");
                } else {
                    // Handle errors
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model) {
            if (ModelState.IsValid) {
                var login = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
                if(!login.Succeeded)
                    return Unauthorized();

                var result = AuthenticationConfiguration.GenerateJwtToken(model.Username);

                return Ok(result);
            }
            return BadRequest(ModelState);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok("User logged out successfully.");
        }
    }
}