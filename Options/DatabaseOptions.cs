using System;

namespace AuthApp.Options;

public class DatabaseOptions
{
    public const string SectionName = "Database";
    public string ConnectionString { get; set; } = "";
    public int CommandTimeout { get; set; } = 30;
    public bool EnableLogging { get; set; } = false;
}
