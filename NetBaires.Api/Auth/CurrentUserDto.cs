using NetBaires.Data;

namespace NetBaires.Api.Auth
{
    public class CurrentUserDto
    {
        public int Id { get; }
        public string Email { get; }
        public UserRole Rol { get; set; }
        public CurrentUserDto(string email, int id, UserRole rol)
        {
            Email = email;
            Id = id;
            Rol = rol;
        }

    }
}