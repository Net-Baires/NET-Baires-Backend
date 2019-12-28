using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetBaires.Data
{
   
    public enum EventPlatform
    {
        [EnumMember(Value = "Meetup")]
        Meetup,
        [EnumMember(Value = "EventBrite")]
        EventBrite
    }
}