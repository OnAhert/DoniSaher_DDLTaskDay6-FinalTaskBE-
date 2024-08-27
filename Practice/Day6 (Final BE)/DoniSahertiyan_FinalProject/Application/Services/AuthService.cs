using System;
using System.Collections.Concurrent;
using Application.Services;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public TokenResponse Authenticate(string username, string password)
        {
            if ((username == "admin" && password == "admin") ||
                (username == "user" && password == "user"))
            {
                return _jwtTokenService.GenerateToken(username, username == "admin" ? "Admin" : "User");
            }
            return null;
        }

        public TokenResponse RefreshToken(string username, string refreshToken)
        {
            if (_jwtTokenService.ValidateRefreshToken(username, refreshToken))
            {
                return _jwtTokenService.GenerateToken(username, username == "admin" ? "Admin" : "User");
            }
            return null;
        }

        public void Logout(string username)
        {
            _jwtTokenService.RevokeRefreshToken(username);
        }
    }
}
