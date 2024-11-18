using Microsoft.AspNetCore.Mvc;
using Core;
using Core.Interfeses;
using Presistence.Contracts;
using Microsoft.AspNetCore.Authorization;
using Aplication;
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

        //[Authorize]
        [Route("all")]
        [HttpGet]
        public async Task<IResult> GetAllUsersAsync()
        {
            var user = await _userRepository.GetUsersAsync();
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [HttpGet ("{id}")]
        public async Task<IResult> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [Route("sign-up")]
        [HttpPost]
        public async Task<IResult> SignUpAsync([FromBody] CreateUser reqest)
        {
            var user = await _userService.CreateNewUserAsync(reqest);

            if (user != null)
            {
                var token = await _userService.CreateTokenAsync(user);
                var response = new
                {
                    Token = token
                };

                return Results.Json(response);
            }            
            return Results.StatusCode(401);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IResult> LoginAsync([FromBody] LoginUser reqest)
        {
            var user = await _userService.CheckUserAsync(reqest);
            if (user != null)
            {
                var token = await _userService.CreateTokenAsync(user);
                var response = new
                {
                    Token = token
                };

                return Results.Json(response);
            }
            return Results.StatusCode(401);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUserAsync(Guid id)
        {
            User user = await _userRepository.RemoveUserAsync(id);
            if (user != null)
                return Results.Ok();
            return Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> UpdateUserAsync(User user)
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
