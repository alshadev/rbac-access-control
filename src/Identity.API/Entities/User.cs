namespace Identity.API.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = "123456";
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
