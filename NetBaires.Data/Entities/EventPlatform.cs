using System.Runtime.Serialization;

namespace NetBaires.Data.Entities
{
   
    public enum EventPlatform
    {
        [EnumMember(Value = "Meetup")]
        Meetup,
        [EnumMember(Value = "EventBrite")]
        EventBrite
    }
}