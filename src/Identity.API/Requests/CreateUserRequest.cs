namespace Identity.API.Requests;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Guid> RoleIds { get; set; }
}
