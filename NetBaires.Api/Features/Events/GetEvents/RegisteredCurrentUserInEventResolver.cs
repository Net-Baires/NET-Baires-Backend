using AutoMapper;
using NetBaires.Api.Auth;
using NetBaires.Data;
using System.Linq;

namespace NetBaires.Api.Handlers.Events
{
    public class RegisteredCurrentUserInEventResolver : IValueResolver<Event, GetEventsResponse, bool>
    {
        ICurrentUser _currentUser;
        public RegisteredCurrentUserInEventResolver(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public bool Resolve(Event source, GetEventsResponse destination, bool destMember, ResolutionContext context)
        {
            return source.Attendees.Any(a => a.MemberId == _currentUser.User.Id);
        }
    }

}