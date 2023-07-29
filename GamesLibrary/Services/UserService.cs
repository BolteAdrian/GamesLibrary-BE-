using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamesLibrary.DataAccessLayer.Contacts;
using GamesLibrary.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static GamesLibrary.Utils.Constants.ResponseConstants;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace GamesLibrary.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return users.Select(u => new UserDto
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    Id = u.Id
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(UNKNOWN, ex);
            }
        }

        public async Task<UserDto> GetUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    throw new Exception(string.Format(USER.NOT_FOUND, id));
                }

                return new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(UNKNOWN, id), ex);
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(IdentityUser user, string password)
        {
            try
            {
                // Manually hash the password
                var passwordHasher = new PasswordHasher<IdentityUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, password);

                // Add the user to the database using the UserManager
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "CLIENT");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(USER.NOT_SAVED, ex);
            }
        }

        public async Task<SignInResult> LoginUserAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    // Ensure the provided password is hashed before using it for authentication
                    var passwordHasher = new PasswordHasher<IdentityUser>();
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        return await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);
                    }
                }

                return SignInResult.Failed;
            }
            catch (Exception ex)
            {
                throw new Exception(UNKNOWN, ex);
            }
        }

        public async Task<bool> DeleteAccount(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception(string.Format(USER.NOT_FOUND, userId));

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(USER.ERROR_REMOVING_USER, userId), ex);
            }
        }

        public async Task<bool> UpdateEmail(string userId, string newEmail)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                user.Email = newEmail;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(USER.EMAIL_ERROR, userId), ex);
            }
        }

        public async Task<bool> UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new Exception(string.Format(USER.NOT_FOUND, userId));

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(USER.PASSWORD_ERROR, userId), ex);
            }
        }

        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(USER.EMAIL_ERROR, email), ex);
            }
        }

        public string GenerateJwtToken(IdentityUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                  _configuration["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(UNKNOWN, ex);
            }
        }

        // Add this method to get the URL in the service
        public string GetResetPasswordUrl(string email, string resetPasswordToken, IUrlHelper urlHelper)
        {
            try
            {
                return urlHelper.Action("ResetPassword", "User", new { email, token = resetPasswordToken }, urlHelper.ActionContext.HttpContext.Request.Scheme);
            }
            catch (Exception ex)
            {
                throw new Exception(UNKNOWN, ex);
            }
        }

        public async Task SendPasswordResetEmail(string email, string resetPasswordToken, IUrlHelper urlHelper)
        {
            try
            {
                var resetUrl = GetResetPasswordUrl(email, resetPasswordToken, urlHelper);
                var emailMessage = $"Please click the link below to reset your password: \n\n{resetUrl}";

                await _emailSender.SendEmailAsync(email, "Reset Password", emailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(EMAIL.ERROR_SENDING, ex);
            }
        }
    }
}
