using System;

namespace AuthApp.Options;

public class UserServiceOptions
{
    public const string SectionName = "UserService";
    public int RecentUsersDays { get; set; } = 30;
}
