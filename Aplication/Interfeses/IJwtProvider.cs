
namespace Core.Interfeses
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
