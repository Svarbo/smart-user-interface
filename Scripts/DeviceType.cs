using System;

namespace SmartUserInterface
{
    [Flags]
    public enum DeviceType
    {
        None = 0,
        Desktop = 1,
        Mobile = 2,
        Console = 4
    }
}