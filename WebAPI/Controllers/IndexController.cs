using Microsoft.AspNetCore.Mvc;
using Core;
using Aplication;
using Core.Interfeses;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class IndexController : ControllerBase
    {
        IUserService userService;
        public IndexController(IUserService userservice)
        {
            userService=userservice;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var u = await Task.Run(()=> userService.GetUsers());
            return Ok(u);
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var u = await Task.Run(() => userService.GetUserById(id));
            return Ok(u);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
                return BadRequest();
            await Task.Run(() =>  userService.Add(user));
            return Ok();
        }
    }
}
