namespace NetBaires.Api.Auth
{
    public interface ICurrentUser
    {
        CurrentUserDto User { get; }
        bool IsLoggued { get; }
    }
}