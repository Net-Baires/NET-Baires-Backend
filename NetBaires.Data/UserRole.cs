using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetBaires.Data
{

    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Organizer")]
        Organizer,
        [EnumMember(Value = "Member")]
        Member
    }

    public enum UserAnonymous
    {
        [EnumMember(Value = "Anonymous")]
        Anonymous
    }
}