using System;

namespace NetBaires.Services
{
    public class UserDoesNotHaveEmailException : Exception
    {
        public int UserId { get; set; }

        public UserDoesNotHaveEmailException(int userId)
        {
            UserId = userId;
        }
    }
}