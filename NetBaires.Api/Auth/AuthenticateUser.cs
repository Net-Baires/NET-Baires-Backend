namespace NetBaires.Api.Auth
{
    public class AuthenticateUser
    {
        public string Token { get; set; }

        public AuthenticateUser(string token)
        {
            Token = token;
        }

    }
}