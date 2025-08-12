using System.Security.Authentication;
using AuthApp.Data;
using AuthApp.DTO;
using AuthApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Services;

public class UserAlreadyExistsException : Exception {  }
public class InternalServerErrorException : Exception {  }


public class UserService : IUserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> ValidateUserUsernameAsync(string username, string password)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userExists == null) { throw new InvalidCredentialException("Invalid Credentials"); }
        ;

        bool checkPass = BCrypt.Net.BCrypt.Verify(password, userExists.PasswordHash);
        if (!checkPass) { throw new InvalidCredentialException("Invalid Credentials"); }
        return userExists;
    }
    public async Task<User?> ValidateUserEmailAsync(string email, string password)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userExists == null) { throw new InvalidCredentialException("Invalid Credentials"); }
        ;

        bool checkPass = BCrypt.Net.BCrypt.Verify(password, userExists.PasswordHash);
        if (!checkPass) { throw new InvalidCredentialException("Invalid Credentials"); }
        return userExists;
    }
    public async Task<User?> CreateUserAsync(string fullname, string username, string email, string password, DateTime _created)
    {
        var userExistsbyUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        var userExistsbyEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userExistsbyUsername != null || userExistsbyEmail != null)
        {
            throw new UserAlreadyExistsException();
        }
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new User
        {
            FullName = fullname,
            Username = username,
            Email = email,
            PasswordHash = hashedPassword,
            created=_created
        };
        try
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

        }
        catch (Exception)
        {
            throw new InternalServerErrorException();
        }
        return newUser;
    }
    // public async Task<User?> GetUser(string userIdentifier)
    // {

    //     var userID = _context.Users.FirstOrDefaultAsync
    // }
}