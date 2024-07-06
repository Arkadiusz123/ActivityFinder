using ActivityFinder.Server.Models;
using ActivityFinder.Server.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IEmailSender _emailSender;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            ITokenService tokenService, ILogger<AuthenticateController> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                DateTime expireTime;

                var token = _tokenService.GenerateToken(user.UserName, userRoles);
                var refreshToken = _tokenService.GenerateRefreshToken(out expireTime);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = expireTime;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"User {user.UserName} successfully logged in");
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    refreshToken
                });
            }
            return Unauthorized("Niepoprawny login lub hasło");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel request)
        {
            if (request == null)           
                return BadRequest("Nieprawidłowe dane");            

            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            var username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);          

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime <= DateTime.Now)            
                return BadRequest("Nieprawidłowe dane");
            
            var userRoles = await _userManager.GetRolesAsync(user);
            DateTime expireTime;

            var newAccessToken = _tokenService.GenerateToken(username, userRoles);
            var newRefreshToken = _tokenService.GenerateRefreshToken(out expireTime);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = expireTime;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                expiration = newAccessToken.ValidTo,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest("Użytkownik istnieje");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest("Nieprawidłowy login lub hasło");            

            await _roleManager.UpdateRoles();
            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new {Message = "Utworzono nowego użytkownika" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("register-admin")]        
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest("Użytkownik istnieje");

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest("Nieprawidłowe dane do rejestracji");
                

            await _roleManager.UpdateRoles();
            await _userManager.AddToRoleAsync(user, UserRoles.User);
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new { Message = "Utworzono nowego użytkownika" });
        }

        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Nieprawidłowy email");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{Request.Scheme}://{Request.Host.Value}/reset-password?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(user.Email)}";

            await _emailSender.SendEmailAsync(model.Email, "Reset hasła", $"<a href='{resetLink}'>Otwórz link, aby zmienić hasło</a>");

            return Ok();
        }

        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Nieprawidłowy email");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!resetPassResult.Succeeded)            
                return BadRequest("Wystąpił błąd. Spróbuj ponownie");
            
            return Ok();
        }
    }
}
