using System.ComponentModel.DataAnnotations;

namespace OrderPickingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Username { get; set; }// TODO Replace Email with Username in JWT
    [Required] public string PasswordHash { get; set; }
    [Required] public string Email { get; set; } // TODO Swap in the JWT for the username
}