using AuthApp.Models;

namespace AuthApp.Services;

public interface IUserService
{
    public Task<User?> ValidateUserEmailAsync(string email, string password);
    public Task<User?> ValidateUserUsernameAsync(string username, string password);
    public Task<User?> CreateUserAsync(string fullname, string username, string email, string password, DateTime created);
    //public Task<User?> GetRecentUsers();
   // public Task<User?> GetUser(string userIdentifier);
}