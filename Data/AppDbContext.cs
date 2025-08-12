using System;
using Microsoft.EntityFrameworkCore;
using AuthApp.Models;

namespace AuthApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Configure unique constraints
    //     modelBuilder.Entity<User>()
    //         .HasIndex(u => u.Username)
    //         .IsUnique();

    //     modelBuilder.Entity<User>()
    //         .HasIndex(u => u.Email)
    //         .IsUnique();

    //     base.OnModelCreating(modelBuilder);
    // }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(e => e.Username).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}
