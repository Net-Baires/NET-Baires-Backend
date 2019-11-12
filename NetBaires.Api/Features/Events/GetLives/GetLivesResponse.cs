using NetBaires.Api.Features.Events.ViewModels;

namespace NetBaires.Api.Handlers.Events
{
    public class GetLivesResponse : EventDetailViewModel
    {
        public bool Registered { get; set; }
    }
}