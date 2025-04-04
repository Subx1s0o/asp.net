using DB.Models;
using Dto;
using Libs.Exceptions;
using Repositories;

namespace Services;

public class AuthService(UserRepository repository, PasswordService passwordService, JwtService jwtService)
{
    public async Task<Dictionary<string, object>> Login(LoginDto userDto)
    {
        var user = await repository.FindOne(x => x.Email == userDto.Email) ?? throw new HttpException("User not found", 404);

        if (passwordService.VerifyPassword(userDto.Password, user.Password))
        {
            string accessToken = GenerateToken(user.Id.ToString(), user.Email);
            string refreshToken = GenerateToken(user.Id.ToString(), user.Email, 60 * 24 * 7);

            return new Dictionary<string, object>
        {
            { "user",
                new
                {
                    user.Email,
                    user.Name,
                    user.Nickname
                }
            },
            { "accessToken", accessToken },
            { "refreshToken", refreshToken }
        }
        ;
        }
        else
        {
            throw new HttpException("Invalid password", 400);
        }
    }

    public async Task<Dictionary<string, object>> Register(RegisterDto userDto)
    {

        var user = await repository.FindOne(x => x.Email == userDto.Email);
        if (user != null)
        {
            throw new HttpException("User already exists", 409);
        }


        user = new UserModel
        {
            Email = userDto.Email,
            Name = userDto.Name,
            Nickname = userDto.Nickname,
            Password = passwordService.HashPassword(userDto.Password)
        };

        await repository.Create(user);

        string accessToken = GenerateToken(user.Id.ToString(), user.Email);
        string refreshToken = GenerateToken(user.Id.ToString(), user.Email, 60 * 24 * 7);
        return new Dictionary<string, object>
    {
        { "user", user },
        { "accessToken", accessToken },
        { "refreshToken", refreshToken }
    };
    }


    public Dictionary<string, object> Refresh(RefreshDto refreshToken)
    {
        var payload = jwtService.ValidateToken(refreshToken.RefreshToken)
                      ?? throw new HttpException("Invalid refresh token", 400);

        var userId = payload["sub"]?.ToString();
        var email = payload["email"]?.ToString();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
        {
            throw new HttpException("Invalid token data", 400);
        }

        string newAccessToken = GenerateToken(userId, email);

        return new Dictionary<string, object>
        {
            { "accessToken", newAccessToken }
        };
    }


    private string GenerateToken(string userId, string email, int expirationMinutes = 60)
    {
        return jwtService.GenerateToken(userId, email, expirationMinutes);
    }
}