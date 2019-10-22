using System;

namespace NetBaires.Data
{
    [Flags]
    public enum UserRole
    {
        Admin,
        Organizer,
        Member
    }
}