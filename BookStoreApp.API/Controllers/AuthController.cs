using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;


        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._userManager = userManager;
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
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            _logger.LogInformation("Login Attempt for {Email}", userDto.Email);

            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (user == null || passwordValid == false)
                {
                    return NotFound();
                }

                return Accepted();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in AuthController.Login()");
                return Problem(ex.Message);
            }   
        }
    }
}
