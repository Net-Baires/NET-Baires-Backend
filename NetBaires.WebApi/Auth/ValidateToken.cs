using System.Collections.Generic;
using System.Security.Claims;

namespace NetBaires.Api.Auth
{
    public class ValidateToken
    {
        public List<Claim> Claims { get; set; }
        public bool Valid { get; set; }
    }
}