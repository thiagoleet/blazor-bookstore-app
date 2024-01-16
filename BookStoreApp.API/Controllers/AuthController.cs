using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;


        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {

            try
            {
                if (userDto == null)
                {
                    return BadRequest("Insuficient Data Provided");
                }

                _logger.LogInformation("Registration Attempt for {Email}", userDto.Email);

                var user = this._mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;


                var result = await this._userManager.CreateAsync(user, userDto.Password);

                // Creating a new user
                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest(ModelState);
                }

                // Setting a role
                await _userManager.AddToRoleAsync(user, "User");


                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthController.Register()");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
        {
            _logger.LogInformation("Login Attempt for {Email}", userDto.Email);

            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (user == null || passwordValid == false)
                {
                    return Unauthorized(userDto);
                }

                string tokenString = await GenerateToken(user);

                var response = new AuthResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Token = tokenString
                };
                
                return Accepted(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthController.Login()");
                return Problem(ex.Message);
            }
        }


        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
