using Aplication.Interfeses;
using Core;
using Core.Interfeses;
using Presistence.Contracts;

namespace Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
