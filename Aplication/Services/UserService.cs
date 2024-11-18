using Aplication.Interfeses;
using Aplication.Repository;
using Core;
using Core.Interfeses;
using Presistence;
using Presistence.Contracts;

namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public UserService(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<User> CreateNewUserAsync(CreateUser reqest)
        {
            var existUser = await _userRepository.GetUserByEmailAsync(reqest.Email);
            if ((existUser == null) && (reqest.Name != null) && (reqest.Email != null) && (reqest.Password != null))
            {
                User user = new User(reqest.Name, reqest.Email, reqest.Password);
                await _userRepository.AddUserAsync(user);
                return user;
            }
            return null;
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task<User> CheckUserAsync(LoginUser reqest)
        {
            var user = await _userRepository.GetUserByEmailAsync(reqest.Email);
            if ((user != null) && (reqest.Password == user.Password))
            {
                return user;
            }
            return null;
        }
    }
}
