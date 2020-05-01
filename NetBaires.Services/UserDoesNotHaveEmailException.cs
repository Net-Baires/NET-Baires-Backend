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

    public class EventDoesNotHaveSpeakers : Exception
    {
        public int EventId { get; set; }

        public EventDoesNotHaveSpeakers(int eventId)
        {

        }
    }
    public class EventDoesNotHaveSponsors : Exception
    {
        public int EventId { get; set; }

        public EventDoesNotHaveSponsors(int eventId)
        {

        }
    }
}