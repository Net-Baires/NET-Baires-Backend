namespace NetBaires.Api.Auth
{
    public interface ICurrentUser
    {
        CurrentUserDto User { get; }
        bool IsLoggued { get; }
    }

    public interface ICurrentUserSignalR
    {
        CurrentUserDto User { get; }
        bool IsLoggued { get; }
    }
}