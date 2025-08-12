using System;

namespace AuthApp.Services;

public class SystemCLK : ISystemCLK
{
    public DateTime TimeNow()
    {
        return DateTime.UtcNow;
    }
}
