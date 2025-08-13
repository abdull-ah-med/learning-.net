using System.Security.Authentication;
using AuthApp.Data;
using AuthApp.DTO;
using AuthApp.Models;
using AuthApp.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;

namespace AuthApp.Services;

public class UserAlreadyExistsException : Exception {  }
public class InternalServerErrorException : Exception {  }


public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IOptions<UserServiceOptions> _uso;
    public UserService(AppDbContext context, IOptions<UserServiceOptions> uso)
    {
        _context = context;
        _uso = uso;
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
    // LINQ Learning Example: Get recent users with specific fields
    public async Task<List<RecentUserDto>> GetRecentUsersAsync(int page = 1, int pageSize = 10)
    {
        var recentUsers = await _context.Users
            .Where(u => u.created > DateTime.Now.AddDays(-_uso.Value.RecentUsersDays))   // Filter: Last 30 days
            .OrderByDescending(u => u.created)                   // Sort: Newest first  
            .Select(u => new RecentUserDto                       // Project: Only specific fields
            { 
                Username = u.Username, 
                Email = u.Email,
                CreatedDate = u.created
            }).Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync();
        
        return recentUsers;
    }
    public async Task<User?> CreateUserAsync(string fullname, string username, string email, string password, DateTime _created)
    {
        // Optimized: Check both username and email in a single query
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
        if (existingUser != null)
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
            created = _created
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