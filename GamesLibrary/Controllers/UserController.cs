using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.DataAccessLayer.Contacts;
using GamesLibrary.DataAccessLayer.Models;
using GamesLibrary.Services;

namespace GamesLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserService _userService;
        private readonly IEmailSender _emailSender;

        public UserController( SignInManager<IdentityUser> signInManager, UserService userService, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userService = userService;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var password = model.Password;

            var result = await _userService.RegisterUserAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok("Registration successful.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _userService.LoginUserAsync(model.Email, model.Password);
            if (result.Succeeded)
            {
                return Ok("Login successful.");
            }

            return BadRequest("Invalid login attempt.");
        }


        [HttpDelete("delete/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var result = await _userService.DeleteAccount(userId);
            if (result)
            {
                return Ok("Account deleted successfully.");
            }

            return BadRequest("Failed to delete account.");
        }

        [HttpPut("update-email/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail(string userId, [FromBody] string newEmail)
        {
            var result = await _userService.UpdateEmail(userId, newEmail);
            if (result)
            {
                return Ok("Email updated successfully.");
            }

            return BadRequest("Failed to update email.");
        }

        [HttpPut("update-password/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string userId, [FromBody] UpdatePasswordModel model)
        {
            var result = await _userService.UpdatePassword(userId, model.CurrentPassword, model.NewPassword);
            if (result)
            {
                return Ok("Password updated successfully.");
            }

            return BadRequest("Failed to update password.");
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
                return BadRequest("User not found");

            var resetPasswordToken = _userService.GenerateJwtToken(user);

            // Utilizarea directă a metodei Url.Action pentru a genera URL-ul
            var resetUrl = Url.Action("ResetPassword", "User", new { email, token = resetPasswordToken }, Request.Scheme);

            var emailMessage = $"Please click the link below to reset your password: \n\n{resetUrl}";

            await _emailSender.SendEmailAsync(email, "Reset Password", emailMessage);

            return Ok("Password reset email sent");
        }
    }
}
