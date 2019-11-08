using System.Runtime.Serialization;

namespace NetBaires.Api.Services
{
    public enum Container
    {
        [EnumMember(Value = "badges")]
        Badges,
        [EnumMember(Value = "sponsors")]
        Sponsors,
        [EnumMember(Value = "members")]
        Members
    }
}
