using System.Linq;
using AutoMapper;
using NetBaires.Api.Auth;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEventLiveDetail
{
    public class GetEventLiveDetailCurrentUserResolver : IValueResolver<Event, GetEventLiveDetailQuery.Response, bool>
    {
        ICurrentUser _currentUser;
        public GetEventLiveDetailCurrentUserResolver(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public bool Resolve(Event source, GetEventLiveDetailQuery.Response destination, bool destMember, ResolutionContext context)
        {
            return source.Attendees.Any(a => a.MemberId == _currentUser.User.Id
                                             &&
                                             a.Attended);
        }
    }

    public class HadGroupCodeOpenResolver : IValueResolver<Event, GetEventLiveDetailQuery.Response, bool>
    {
        ICurrentUser _currentUser;
        public HadGroupCodeOpenResolver(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        public bool Resolve(Event source, GetEventLiveDetailQuery.Response destination, bool destMember, ResolutionContext context)
        {
            return source.GroupCodes.Any(a => a.Open && !a.Members.Any(m => m.MemberId  == _currentUser.User.Id));
        }
    }
}