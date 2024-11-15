using Microsoft.AspNetCore.Mvc;
using Core;
using Aplication;
using Core.Interfeses;
using System.Text.Json;
using Presistence.Contracts;
using System.Buffers;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService userService;
        IJwtProvider jwtProvider;

        public UserController(IUserService userservice, IJwtProvider jwtprovider)
        {
            userService = userservice;
            jwtProvider = jwtprovider;
        }

        [Authorize]
        [HttpGet]
        public async Task<IResult> Get()
        {
            var u = await Task.Run(() => userService.GetUsers());
            if (u == null) return Results.BadRequest();
            return Results.Json(u);
        }

        [HttpGet ("{id}")]
        public async Task<IResult> Get(int id)
        {
            var u = await Task.Run(() => userService.GetUserById(id));
            if (u == null) return Results.BadRequest();
            return Results.Json(u);
        }

        [Route("SingUp")]
        [HttpPost]
        public async Task<IResult> PostSingUp([FromBody] CreateUser reqest)
        {
            var u = await Task.Run(() => userService.GetUserByEmail(reqest.Email));
            if (u != null) 
                return Results.StatusCode(401);

            if ((reqest.Name == null) || (reqest.Email == null) || (reqest.Password == null))
                return Results.BadRequest();

            User user = new User(reqest.Name, reqest.Email, reqest.Password);
          
            var b = await Task.Run(() => userService.Add(user));
            if (b)
            {
                var token = jwtProvider.GenerateToken(user);
                var response = new
                {
                    Token = token
                };

                return Results.Json(response);
            }
            return Results.BadRequest();
        }

        [Route("LogIn")]
        [HttpPost]
        public async Task<IResult> PostLogIn([FromBody] CreateUser reqest)
        {
            var user = await Task.Run(() => userService.GetUserByEmail(reqest.Email));
            if (user == null) 
                return Results.StatusCode(401);

            if ((reqest.Name != user.Name) || (reqest.Password != user.Password))
                return Results.StatusCode(401);

            var token = jwtProvider.GenerateToken(user);
            var response = new
            {
                Token = token
            };

            return Results.Json(response);
        }

        [HttpDelete]
        public async Task<IResult> Delete(User user)
        {
            if (user == null)
                return Results.BadRequest();
            var b = await Task.Run(() => userService.Remove(user));
            if (b)
                return Results.Ok();
            return Results.BadRequest();
        }

        [HttpPut]
        public async Task<IResult> Put(User user)
        {
            if (user == null)
                return Results.BadRequest();
            var b = await Task.Run(() => userService.Update(user));
            if (b)
                return Results.Ok();
            return Results.BadRequest();
        }
    }
}
