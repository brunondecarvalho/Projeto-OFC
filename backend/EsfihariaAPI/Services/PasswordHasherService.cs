using EsfihariaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace EsfihariaAPI.Services;

public interface IPasswordHashService
{
    string Hash(User user, string password);

    bool Verify(
        User user,
        string hash,
        string password
    );
}

public class PasswordHashService
    : IPasswordHashService
{
    private readonly PasswordHasher<User> _hasher
        = new();

    public string Hash(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }

    public bool Verify(
        User user,
        string hash,
        string password)
    {
        return _hasher.VerifyHashedPassword(
            user,
            hash,
            password
        ) == PasswordVerificationResult.Success;
    }
}