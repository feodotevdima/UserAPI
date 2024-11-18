using Core;
using Presistence.Contracts;

namespace Aplication.Interfeses
{
    public interface IUserService
    {
        Task<User> CreateNewUserAsync(CreateUser reqest);
        Task<string> CreateTokenAsync(User user);
        Task<User> CheckUserAsync(LoginUser reqest);
    }
}
