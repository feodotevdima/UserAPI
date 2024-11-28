using Microsoft.AspNetCore.Mvc;
using Core;
using Core.Interfeses;
using Presistence.Contracts;
using Aplication.Interfeses;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [Route("all")]
        [HttpGet]
        public async Task<IResult> GetAllUsersAsync()
        {
            var user = await _userRepository.GetUsersAsync();
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [HttpGet ("id/{id}")]
        public async Task<IResult> GetUserAsync([FromBody] Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [HttpGet("login/{login}")]
        public async Task<IResult> GetUserByLoginAsync([FromBody] string login)
        {
            var user = await _userRepository.GetUserByEmailAsync(login);
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IResult> AddUserAsync([FromBody] CreateUser reqest)
        {
            var user = await _userService.CreateNewUserAsync(reqest);

            if (user != null)
                return Results.Json(user);     

            return Results.StatusCode(401);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUserAsync([FromBody] Guid id)
        {
            User user = await _userRepository.RemoveUserAsync(id);
            if (user != null)
                return Results.Ok();
            return Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> UpdateUserAsync([FromBody] User user)
        {
            if (user != null)
            {
                User newUser = await _userRepository.UpdateUserAsync(user);
                if (newUser != null)
                    return Results.Ok();
            }
            return Results.BadRequest();
        }
    }
}
