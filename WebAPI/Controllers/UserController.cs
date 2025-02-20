using Microsoft.AspNetCore.Mvc;
using Core;
using Core.Interfeses;
using Presistence.Contracts;
using Aplication.Interfeses;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly string _targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
            if (!Directory.Exists(_targetFilePath))
            {
                Directory.CreateDirectory(_targetFilePath);
            }
        }

        [Route("all")]
        [HttpGet]
        public async Task<IResult> GetAllUsersAsync()
        {
            var user = await _userRepository.GetUsersAsync();
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }


        [HttpGet("{token}")]
        public async Task<IResult> GetUserByTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                string? userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                Guid.TryParse(userId, out var Id);
                var user = await _userRepository.GetUserByIdAsync(Id);
                if (user == null) return Results.BadRequest();
                return Results.Json(user);
            }
            return Results.BadRequest();
        }

        [HttpGet ("id/{id}")]
        public async Task<IResult> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [HttpGet("login/{login}")]
        public async Task<IResult> GetUserByLoginAsync(string login)
        {
            var user = await _userRepository.GetUserByEmailAsync(login);
            if (user == null) return Results.BadRequest();
            return Results.Json(user);
        }

        [HttpGet("images/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine("wwwroot", imageName);
            if (!System.IO.File.Exists(imagePath))
                return NotFound();

            var imageFileStream = System.IO.File.OpenRead(imagePath);
            return File(imageFileStream, "image/jpeg");
        }

        [Route("add")]
        [HttpPost]
        public async Task<IResult> AddUserAsync(CreateUser reqest)
        {
            var user = await _userService.CreateNewUserAsync(reqest);

            if (user != null)
                return Results.Json(user);     

            return Results.StatusCode(400);
        }

        [HttpPost]
        [Authorize]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var token = Request.Headers["Authorization"].ToString();
            token = token.Substring(7);
            var userIdStr = _userService.GetUserIdFromToken(token);

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileExtension = Path.GetExtension(file.FileName);

            var newFileName = $"{userIdStr}{fileExtension}";

            var filePath = Path.Combine(_targetFilePath, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Guid.TryParse(userIdStr, out var id);
            var user = await _userRepository.GetUserByIdAsync(id);
            user.Image = "User/images/"+newFileName;
            var u = await _userRepository.UpdateUserAsync(user);

            return Ok(new { FilePath = filePath });
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUserAsync(Guid id)
        {
            User user = await _userRepository.RemoveUserAsync(id);
            if (user != null)
                return Results.Ok();
            return Results.BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateUserAsync(string id, [FromBody] String name)
        {
            if (name != null)
            {
                Guid.TryParse(id, out var IdGuid);
                var user = await _userRepository.GetUserByIdAsync(IdGuid);
                user.Name = name;
                User newUser = await _userRepository.UpdateUserAsync(user);
                if (newUser != null)
                    return Results.Ok();
            }
            return Results.BadRequest();
        }
    }
}
