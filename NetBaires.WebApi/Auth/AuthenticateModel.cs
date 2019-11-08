using System.ComponentModel.DataAnnotations;

namespace NetBaires.Api.Auth
{
    public class AuthenticateModel
    {
        [Required]
        public string Token { get; set; }
    }
}