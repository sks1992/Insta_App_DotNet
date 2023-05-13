using Insta_App.DataAccess.Repository.IRepository;
using Insta_App.Models;
using Insta_App.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Insta_App_API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDTO createUser)
        {
            var createUserResponse = await _userRepository.RegisterUser(createUser);
            return Ok(createUserResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser)
        {
            var loginResponse = await _userRepository.Login(loginUser);
            return Ok(loginResponse);
        }
    }
}
