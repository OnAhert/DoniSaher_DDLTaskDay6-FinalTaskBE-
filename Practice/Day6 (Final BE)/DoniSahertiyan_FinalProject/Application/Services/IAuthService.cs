using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {
        TokenResponse Authenticate(string username, string password);
        TokenResponse RefreshToken(string username, string refreshToken);
        void Logout(string username);
    }
}
