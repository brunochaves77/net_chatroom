using ChatRoom.Application.Models;
using ChatRoom.Application.Models.Requests;
using ChatRoom.Application.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatRoom.Application.Services {
    public class IdentityService {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<IdentityUser> signInManager,
                                UserManager<IdentityUser> userManager,
                                IOptions<JwtOptions> options) {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = options.Value;
        }

        public async Task<UserRegistrationResponse> Register(UserRegistrationRequest userRegistrationRequest) {
            var identityUser = new IdentityUser {
                UserName = userRegistrationRequest.Username,
            };

            var result = await _userManager.CreateAsync(identityUser, userRegistrationRequest.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(identityUser, false);

            var userRegisterResponse = new UserRegistrationResponse(result.Succeeded);
            if (!result.Succeeded && result.Errors.Count() > 0)
                userRegisterResponse.AddErrors(result.Errors.Select(r => r.Description));

            return userRegisterResponse;
        }

        public async Task<UserLoginResponse> Login(LoginRequest userLogin) {
            var result = await _signInManager.PasswordSignInAsync(userLogin.Username, userLogin.Password, false, true);
            if (result.Succeeded)
                return await GenerateCredentials(userLogin.Username);

            var userLoginResponse = new UserLoginResponse();
            if (!result.Succeeded) {
                if (result.IsLockedOut)
                    userLoginResponse.AddError("This account is locked");
                else if (result.IsNotAllowed)
                    userLoginResponse.AddError("This account is not allowed to login");
                else if (result.RequiresTwoFactor)
                    userLoginResponse.AddError("Login needs to be confirmed on your second factor authentication");
                else
                    userLoginResponse.AddError("Username or password are incorrect");
            }

            return userLoginResponse;
        }

        private async Task<UserLoginResponse> GenerateCredentials(string userName) {
            var user = await _userManager.FindByNameAsync(userName);
            var accessTokenClaims = await GetClaims(user, addUserClaims: true);
            var refreshTokenClaims = await GetClaims(user, addUserClaims: false);

            var accessTokenExpirationDate = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
            var refreshTokenExpirationDate = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

            var accessToken = GenerateToken(accessTokenClaims, accessTokenExpirationDate);
            var refreshToken = GenerateToken(refreshTokenClaims, refreshTokenExpirationDate);

            return new UserLoginResponse
            (
                success: true,
                accessToken: accessToken,
                refreshToken: refreshToken
            );
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate) {
            var jwt = new JwtSecurityToken(
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expirationDate,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> GetClaims(IdentityUser user, bool addUserClaims) {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));


            return claims;
        }

    }
}
