using System;

namespace Identity.API.Requests;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
