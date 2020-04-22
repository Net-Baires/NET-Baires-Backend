using AutoMapper;

namespace NetBaires.Api.Features.EventInformation.GetEventInformation
{
    public class EventInformationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
        public class EventInformationEventInformationViewModelProfile : Profile
        {
            public EventInformationEventInformationViewModelProfile()
            {
                CreateMap<Data.Entities.EventInformation, EventInformationViewModel>();
            }
        }
    }
}