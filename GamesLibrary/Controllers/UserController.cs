using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GamesLibrary.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesLibrary.Repository.Interfaces;

namespace GamesLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController( SignInManager<IdentityUser> signInManager, UserService userService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the user to have the "ManagerOnly" authorization policy.
        /// </remarks>
        /// <returns>
        /// A list of all users if successful, otherwise an error response.
        /// </returns>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();

                if (users == null)
                {
                    return NotFound();
                }
                return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// The user's information if found, otherwise a NotFound response.
        /// </returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Invalid ID.");
                }

                var user = await _userService.GetUserById(id);

            return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration information.</param>
        /// <returns>
        /// If successful, returns "Registration successful."
        /// If unsuccessful, returns the errors in a BadRequest response.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
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
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>
        /// If successful, returns "Login successful."
        /// If unsuccessful, returns a BadRequest response with the message "Invalid login attempt."
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _userService.LoginUserAsync(model.Email, model.Password);
                if (result.Succeeded)
                {
                    return Ok("Login successful.");
                }

                return BadRequest("Invalid login attempt.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the account of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user account to delete.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// If successful, returns "Account deleted successfully."
        /// If unsuccessful, returns a BadRequest response with the message "Failed to delete account."
        /// </returns>
        [HttpDelete("delete/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("Invalid ID.");
                }
                var result = await _userService.DeleteAccount(userId);
                if (result)
                {
                    return Ok("Account deleted successfully.");
                }

                return BadRequest("Failed to delete account.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates the email address of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose email is being updated.</param>
        /// <param name="newEmail">The new email address to set.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// If successful, returns "Email updated successfully."
        /// If unsuccessful, returns a BadRequest response with the message "Failed to update email."
        /// </returns>
        [HttpPut("update-email/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail(string userId, [FromBody] string newEmail)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("Invalid ID.");
                }
                var result = await _userService.UpdateEmail(userId, newEmail);
                if (result)
                {
                    return Ok("Email updated successfully.");
                }

                return BadRequest("Failed to update email.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates the password of a user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose password is being updated.</param>
        /// <param name="model">The password update information.</param>
        /// <remarks>
        /// This endpoint requires the user to be authorized.
        /// </remarks>
        /// <returns>
        /// If successful, returns "Password updated successfully."
        /// If unsuccessful, returns a BadRequest response with the message "Failed to update password."
        /// </returns>
        [HttpPut("update-password/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string userId, [FromBody] UpdatePasswordDto model)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("Invalid ID.");
                }
                var result = await _userService.UpdatePassword(userId, model.CurrentPassword, model.NewPassword);
                if (result)
                {
                    return Ok("Password updated successfully.");
                }

                return BadRequest("Failed to update password.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Initiates the process of resetting a user's password by sending a reset email.
        /// </summary>
        /// <param name="email">The email address of the user requesting the password reset.</param>
        /// <returns>
        /// If successful, returns "Password reset email sent."
        /// If the user is not found, returns a BadRequest response with the message "User not found."
        /// </returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest("Invalid Email.");
                }

                var user = await _userService.GetUserByEmail(email);
                if (user == null)
                    return BadRequest("User not found");

                var resetPasswordToken = _userService.GenerateJwtToken(user);

                await _userService.SendPasswordResetEmail(email, resetPasswordToken);

                return Ok("Password reset email sent");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        /// <summary>
        /// Resets the user's password using the provided password reset token.
        /// </summary>
        /// <param name="email">The email address of the user whose password will be reset.</param>
        /// <param name="token">The password reset token associated with the user.</param>
        /// <returns>
        /// If the password reset is successful, returns an Ok result with the message "Password reset successful."
        /// If the user is not found or the token is invalid, returns a BadRequest result with the corresponding message.
        /// If an error occurs during password reset, returns a StatusCode 500 result with the message "An error occurred while processing the request."
        /// </returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            try
            {
                if (email == null)
                {
                    return BadRequest("Invalid Email.");
                }

                // Check the validity of the JWT token
                var user = await _userService.GetUserByEmail(email);
                if (user == null)
                    return BadRequest("User not found");

                // Validate the received JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = false, // We don't validate the audience because it's not required in this case
                    ValidateLifetime = true
                };

                // Try to validate the JWT token
                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                }
                catch (Exception ex)
                {
                    return BadRequest("Invalid token");
                }

                // Check if the email in the token matches the user's email
                var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
                if (emailClaim == null || emailClaim.Value != email)
                    return BadRequest("Invalid token");

                // Implement the logic to reset the password and update it in the database
                // For example, you can use UserManager to update the user's password:
                var newPassword = "YourNewPasswordHere"; // You can receive the new password from a form or an action parameter
                var tokenValidTo = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                var validTo = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenValidTo)).UtcDateTime;
                var resetPasswordResult = await _userService.ResetPasswordAsync(user, token, newPassword);
                if (resetPasswordResult.Succeeded)
                {
                    // Password reset was successful
                    return Ok("Password reset successful");
                }
                else
                {
                    // An error occurred during password reset
                    // You can access the error details from resetPasswordResult.Errors
                    return BadRequest("Password reset failed");
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur during the password reset process
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
