using System.ComponentModel.DataAnnotations;

namespace Digital.Lib.Net.Authentication.Controllers.Models;

public class LoginPayload
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}