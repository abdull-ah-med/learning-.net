using System;
using Microsoft.EntityFrameworkCore;
using AuthApp.Models;

namespace AuthApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
