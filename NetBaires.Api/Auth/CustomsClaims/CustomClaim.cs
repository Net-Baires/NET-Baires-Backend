using System;

namespace NetBaires.Api.Auth.CustomsClaims
{
    public class CustomClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public CustomClaim(string type, string value)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}